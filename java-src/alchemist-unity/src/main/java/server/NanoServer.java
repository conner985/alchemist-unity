package server;


import java.io.IOException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Map.Entry;
import java.util.Set;

import org.json.JSONArray;
import org.json.JSONObject;

import com.google.gson.JsonParser;

import fi.iki.elonen.NanoHTTPD;
import it.unibo.alchemist.core.implementations.Engine;
import it.unibo.alchemist.core.interfaces.Simulation;
import it.unibo.alchemist.loader.Loader;
import it.unibo.alchemist.loader.YamlLoader;
import it.unibo.alchemist.model.implementations.nodes.ProtelisNode;
import it.unibo.alchemist.model.implementations.positions.Continuous2DEuclidean;
import it.unibo.alchemist.model.implementations.times.DoubleTime;
import it.unibo.alchemist.model.interfaces.Environment;
import it.unibo.alchemist.model.interfaces.Molecule;
import it.unibo.alchemist.model.interfaces.Node;
import nodes.CustomMonitor;
import nodes.SingleNode;

public class NanoServer extends NanoHTTPD{

	private final Environment<Object> env;
	private final Simulation<Object> sim;

	public NanoServer() throws IOException {
		super(8080);
		start(NanoHTTPD.SOCKET_READ_TIMEOUT, false);
		System.out.println("\nRunning! Point your browsers to http://localhost:8080/ \n");

		String path = "/a.yml";
		final Loader loader = new YamlLoader(NanoServer.class.getResourceAsStream(path));
		System.out.println("loader: "+loader);

		env = loader.getWith(Collections.emptyMap());
		sim = new Engine<Object>(env, DoubleTime.INFINITE_TIME);

		sim.addOutputMonitor(new CustomMonitor());

		new Thread(sim).start();
	}

	public static void main(String[] args) {
		try {
			new NanoServer();
		} catch (IOException ioe) {
			System.err.println("Couldn't start server:\n" + ioe);
		}
	}

	@Override	    
	public Response serve(IHTTPSession session) {

		switch (session.getMethod().name()) {

		case "POST":
			System.out.println("POST Request");
			if (session.getHeaders().get("content-type").equals("application/json")) {
				try {
					Map<String,String> map = new HashMap<String,String>();
					session.parseBody(map);

					String json_string = map.get("postData");
					JSONObject jobj = new JSONObject(json_string); 

					String json_type = jobj.getString("type");

					switch (json_type) {
					case "init":
						int n_nodes = Integer.parseInt(jobj.getString("n_nodes"));
						for (int i = 0; i < n_nodes-1; i++) {
							Node<Object> node = env.getNodeByID(0);
							ProtelisNode p_Node = new ProtelisNode(env);
							p_Node.put("data", 0);
							sim.schedule(()->env.addNode(p_Node, new Continuous2DEuclidean(0,0)));
						}
						break;

					case "step":
						JSONArray jArray = jobj.getJSONArray("nodes");
						for (int i = 0; i < jArray.length(); i++) {
							JSONObject json_object = jArray.getJSONObject(i);
							Node<Object> node = env.getNodeByID(json_object.getInt("id"));
							double posx = json_object.getJSONObject("position").getDouble("posx");
							double posz = json_object.getJSONObject("position").getDouble("posz");
							env.moveNode(node, new Continuous2DEuclidean(posx, posz));
						}
						break;
					}

				} catch (Exception e) { e.printStackTrace();  }
				return newFixedLengthResponse("POST successful");
			}

			break;

		case "GET":
			System.out.println("GET Request");
			try {

				final Collection<Node<Object>> nodes_collection = env.getNodes();
				
				final JSONObject nodes_obj = new JSONObject();
				final JSONArray nodes = new JSONArray();
				
				final Iterator<Node<Object>> nodes_iterator = nodes_collection.iterator();
				while (nodes_iterator.hasNext()) {
					
					final Node<Object> node_obj = nodes_iterator.next();
					final Iterator<Entry<Molecule, Object>> iter = node_obj.getContents().entrySet().iterator();
					while(iter.hasNext()){
						final Entry<Molecule, Object> entry = iter.next();
						final String molecule = entry.getKey().getName();
						if(molecule.equals("data")){
							JSONObject object = new JSONObject();
							object.put("data", entry.getValue());
							object.put("id", node_obj.getId());
							nodes.put(object);
						}
						
					}
					
				}
				
				nodes_obj.put("nodes", nodes);
				
				System.out.println(nodes_obj);
				return newFixedLengthResponse(nodes_obj.toString());
			} catch (Exception e) { e.printStackTrace(); }
			break;

		}




		return newFixedLengthResponse("");
	}
}