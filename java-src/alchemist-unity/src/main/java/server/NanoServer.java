package server;


import java.io.IOException;
import java.lang.reflect.Type;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.concurrent.Semaphore;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.reflect.TypeToken;

import fi.iki.elonen.NanoHTTPD;
import it.unibo.alchemist.boundary.interfaces.OutputMonitor;
import it.unibo.alchemist.core.implementations.Engine;
import it.unibo.alchemist.core.interfaces.Simulation;
import it.unibo.alchemist.loader.Loader;
import it.unibo.alchemist.loader.YamlLoader;
import it.unibo.alchemist.model.implementations.molecules.SimpleMolecule;
import it.unibo.alchemist.model.implementations.times.DoubleTime;
import it.unibo.alchemist.model.interfaces.Environment;
import it.unibo.alchemist.model.interfaces.Molecule;
import it.unibo.alchemist.model.interfaces.Node;
import it.unibo.alchemist.model.interfaces.Reaction;
import it.unibo.alchemist.model.interfaces.Time;
import nodes.InitComm;
import nodes.NodesDescriptor;
import nodes.GradientNode;
import nodes.IMoleculesMap;

/***
 * Server Built with  NanoHTTPD Library to handle a REST communication between alchemist and unity. 
 * 
 * When it receives a POST request of a Json it will try to convert it in a NodesDescriptor through Gson Utility 
 * and then set all the alchemist node present in the environment according with those of the NodeDescriptor.
 * 
 * When it receives a GET request it will collect all the alchemist nodes present on the environment into a NodesDescriptor 
 * and then using the Gson Utility it will covert the NodeDescriptor in a Json string and send it to the requester
 */
public class NanoServer extends NanoHTTPD {

    private static final String DATAMOL = "data";
    private static final Molecule ALCHEMIST_DATAMOL = new SimpleMolecule(DATAMOL);

    private static final Gson GSON_OBJ = new Gson();
    private static final int PORT_NUM = 8080;
    private static final double GRADIENT_MAXVALUE = 1000000;
    private final NodesDescriptor<GradientNode> nodes = new NodesDescriptor<GradientNode>();
    private final Semaphore mutex = new Semaphore(1);

    private ProgType progType;
    private Environment<Object> env;
    private Simulation<Object> sim;

    private enum ProgType { 
        GRADIENT,
        OTHER
    }

    /***
     * Server Constructor: simply set the port number for the socket to 8080 and start the server.
     * @throws IOException if the standard setup of the server hasn't gone well
     */
    public NanoServer() throws IOException {
        super(PORT_NUM);
        start(NanoHTTPD.SOCKET_READ_TIMEOUT, false);
        System.out.println("\nRunning! Point your browsers to http://localhost:8080/ \n");
    }

    /***
     * Create an instance of this class and start it.
     * @param args none is needed
     */
    public static void main(final String[] args) {
        try {
            new NanoServer();
        } catch (IOException ioe) {
            System.err.println("Couldn't start server:\n" + ioe);
        }
    }

    @Override
    public Response serve(final IHTTPSession session) { 

        switch (session.getMethod().name()) {

        case "POST":
            return doPOST(session);

        case "GET":
            return doGET();
        default:
            return newFixedLengthResponse("USE A RESTFUL API!");
        }

    }

    private Response doGET() {
        System.out.println("GET Request");
        while (nodes.getNodesList().isEmpty()) {
            try {
                Thread.sleep(100);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        try {
            mutex.acquireUninterruptibly();
            final String jsonString = GSON_OBJ.toJson(nodes); 
            mutex.release();
            return newFixedLengthResponse(jsonString);
        } catch (Exception e) {
            e.printStackTrace();
            return newFixedLengthResponse("ERROR GET");
        }
    }

    private Response doPOST(final IHTTPSession session) {
        System.out.println("POST Request");
        if (session.getHeaders().get("content-type").equals("application/json")) {
            try {
                final Map<String, String> map = new HashMap<String, String>();
                session.parseBody(map);

                final String jsonString = map.get("postData");

                System.out.println("POST: " + jsonString);

                final JsonObject jsonObj = new JsonParser().parse(jsonString).getAsJsonObject();
                final String jsonType = jsonObj.get("type").getAsString();

                switch (jsonType) {
                case "init":
                    init(jsonObj);
                    //initWithClone(jsonObj);
                    sim.play();
                    break;

                case "step":
                    step(jsonObj);
                    break;

                default:
                    throw new IllegalStateException();
                }

            } catch (Exception e) { 
                e.printStackTrace();
            }
            return newFixedLengthResponse("POST successful");
        }
        return newFixedLengthResponse("NEED A JSON TO PARSE!"); 

    }

    private void step(final JsonObject jsonObj) {
        switch (progType) {
        case GRADIENT:
            Type listType = new TypeToken<NodesDescriptor<GradientNode>>() { }.getType();
            final NodesDescriptor<GradientNode> nodes = GSON_OBJ.fromJson(jsonObj, listType);
            sim.schedule(() -> {
                for (final GradientNode gNode : nodes.getNodesList()) {
                    final Node<Object> node = env.getNodeByID(gNode.getID());
                    IMoleculesMap molecules = gNode.getMolecules();
                    node.setConcentration(new SimpleMolecule("source"), (boolean) molecules.getMoleculeConcentration("source"));
                    node.setConcentration(new SimpleMolecule("enabled"), (boolean) molecules.getMoleculeConcentration("enabled"));
                    env.moveNodeToPosition(node, env.makePosition(gNode.getPosition().getPosx(), gNode.getPosition().getPosz()));
                }
            });
            break;

        default:
            System.err.println("Don't know the program you asked for! : " + progType);
            break;
        }
    }

    @SuppressWarnings("unused")
    private void initWithClone(final JsonObject jsonObj) {
        try {
            progType = ProgType.valueOf(jsonObj.get("progType").getAsString());
        } catch (IllegalArgumentException e) {
            System.err.println("Don't know the program you requested: " + jsonObj.get("program").getAsString());
        }
        switch (progType) {
        case GRADIENT:
            final String pathYaml = "/gradient.yml";
            final Loader loader = new YamlLoader(NanoServer.class.getResourceAsStream(pathYaml));
            env = loader.getWith(Collections.emptyMap());
            sim = new Engine<Object>(env, DoubleTime.INFINITE_TIME);
            sim.addOutputMonitor(new OutputMonitor<Object>() {
                private static final long serialVersionUID = -9149225800059018745L;
                @Override
                public void stepDone(final Environment<Object> env, final Reaction<Object> r, final Time time, final long step) {
                    mutex.acquireUninterruptibly();
                    nodes.clear();
                    env.getNodes().forEach(n -> {
                        final Object conc = n.getConcentration(ALCHEMIST_DATAMOL);
                        if (conc instanceof Number) {
                            final double val;
                            if (Double.isInfinite(((Number) conc).doubleValue())) { 
                                val = GRADIENT_MAXVALUE;
                            } else { 
                                val = ((Number) conc).doubleValue();
                            }
                            GradientNode unityNode = new GradientNode(n.getId());
                            unityNode.setMolecule(DATAMOL, val);
                            nodes.addNode(unityNode);

                        } else {
                            throw new IllegalStateException("Unexpected non-numeric value: " + conc);
                        }
                    });
                    mutex.release();
                }
                @Override
                public void initialized(final Environment<Object> env) {
                    System.out.println("initialized");
                }
                @Override
                public void finished(final Environment<Object> env, final Time time, final long step) {
                    System.out.println("finished"); }
            });
            break;
        default:
            break;
        }

        // cloning nodes in init
        final InitComm comm = GSON_OBJ.fromJson(jsonObj, InitComm.class);
        // place the first node in 0,0 position and remove all the others
        final Node<Object> node0 = env.getNodeByID(0);
        sim.schedule(() -> {
            final int nNodes = comm.getNumNodes();
            env.moveNodeToPosition(node0, env.makePosition(0, 0));

            Collection<Node<Object>> alchemistNodes = env.getNodes();
            Iterator<Node<Object>> it = alchemistNodes.iterator();
            while (it.hasNext()) {
                Node<Object> node = (Node<Object>) it.next();
                if (node.getId() != 0) {
                    env.removeNode(node);
                }
            }

            for (int i = 0; i < nNodes - 1; i++) {
                env.addNode(node0.cloneNode(), env.makePosition(0, 0));
            }
        });
        new Thread(sim).start();
    }

    private void init(final JsonObject jsonObj) {
        try {
            progType = ProgType.valueOf(jsonObj.get("progType").getAsString());
        } catch (IllegalArgumentException e) {
            System.err.println("Don't know the program you requested: " + jsonObj.get("program").getAsString());
        }
        final Loader loader;
        switch (progType) {
        case GRADIENT:
            final String pathYaml = "/gradient2.yml";
            loader = new YamlLoader(NanoServer.class.getResourceAsStream(pathYaml));
            env = loader.getWith(Collections.emptyMap());
            sim = new Engine<Object>(env, DoubleTime.INFINITE_TIME);
            sim.addOutputMonitor(new OutputMonitor<Object>() {
                private static final long serialVersionUID = -9149225800059018745L;
                @Override
                public void stepDone(final Environment<Object> env, final Reaction<Object> r, final Time time, final long step) {
                    mutex.acquireUninterruptibly();
                    nodes.clear();
                    env.getNodes().forEach(n -> {
                        final Object conc = n.getConcentration(ALCHEMIST_DATAMOL);
                        if (conc instanceof Number) {
                            final double val;
                            if (Double.isInfinite(((Number) conc).doubleValue())) { 
                                val = GRADIENT_MAXVALUE;
                            } else { 
                                val = ((Number) conc).doubleValue();
                            }
                            GradientNode unityNode = new GradientNode(n.getId());
                            unityNode.setMolecule(DATAMOL, val);
                            nodes.addNode(unityNode);

                        } else {
                            throw new IllegalStateException("Unexpected non-numeric value: " + conc);
                        }
                    });
                    mutex.release();
                }
                @Override
                public void initialized(final Environment<Object> env) {
                    System.out.println("initialized");
                }
                @Override
                public void finished(final Environment<Object> env, final Time time, final long step) {
                    System.out.println("finished"); }
            });
            break;

        default:
            System.err.println("Don't know the program you asked for! : " + progType);
            break;
        }

        new Thread(sim).start();
    }

}