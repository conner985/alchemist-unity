package it.unibo.alchemist.test;

import java.util.Collections;

import org.junit.Before;
import org.junit.Test;
import static org.junit.Assert.assertTrue;

import it.unibo.alchemist.boundary.interfaces.OutputMonitor;
import it.unibo.alchemist.core.implementations.Engine;
import it.unibo.alchemist.loader.YamlLoader;
import it.unibo.alchemist.model.implementations.molecules.SimpleMolecule;
import it.unibo.alchemist.model.implementations.times.DoubleTime;
import it.unibo.alchemist.model.interfaces.Environment;
import it.unibo.alchemist.model.interfaces.Node;
import it.unibo.alchemist.model.interfaces.Reaction;
import it.unibo.alchemist.model.interfaces.Time;
import nodes.GradientNode;
import server.NanoServer;

/***
 * TODO.
 */
public class TestGradient {

    private Environment<Object> env;
    private Engine<Object> sim;

    private double gradientNode0 = 0f;
    private double gradientNode1 = 0f;
    private double gradientNode2 = 0f;
    private double gradientNode3 = 0f;

    private static final SimpleMolecule SOURCEMOL = new SimpleMolecule("source");
    private static final SimpleMolecule ENABLEDMOL = new SimpleMolecule("enabled");
    private static final SimpleMolecule DATAMOL = new SimpleMolecule("data");

    /***
     * TODO.
     */
    @Before
    public void setUp() {
        final String pathYaml = "/gradient.yml";
        YamlLoader loader = new YamlLoader(NanoServer.class.getResourceAsStream(pathYaml));
        env = loader.getWith(Collections.emptyMap());
        sim = new Engine<Object>(env, DoubleTime.INFINITE_TIME);
        new Thread(sim).start();
    }

    /***
     * TODO.
     * @throws InterruptedException 
     */
    @Test
    public void test() throws InterruptedException {
        sim.schedule(()-> {

            //id=0
            final Node<Object> node0 = env.getNodeByID(0);
            env.moveNodeToPosition(node0, env.makePosition(-30.72191619873047, -9.75));
            node0.setConcentration(SOURCEMOL, false);
            node0.setConcentration(ENABLEDMOL, true);

            //id=1
            final Node<Object> node1 = node0.cloneNode();
            node1.setConcentration(SOURCEMOL, false);
            node1.setConcentration(ENABLEDMOL, true);
            env.addNode(node1, env.makePosition(-34.62321853637695, -6.039149761199951));

            //id=2
            final Node<Object> node2 = node0.cloneNode();
            node2.setConcentration(SOURCEMOL, true);
            node2.setConcentration(ENABLEDMOL, true);
            env.addNode(node2, env.makePosition(-33.585994720458987, -1.3899999856948853));

            //id=3
            final Node<Object> node3 = node0.cloneNode();
            node3.setConcentration(SOURCEMOL, false);
            node3.setConcentration(ENABLEDMOL, true);
            env.addNode(node3, env.makePosition(-26.3700008392334, -9.899999618530274));
        });

        sim.addOutputMonitor(new OutputMonitor<Object>() {
            private static final long serialVersionUID = -9149225800059018745L;
            @Override
            public void stepDone(final Environment<Object> env, final Reaction<Object> r, final Time time, final long step) {
                if (step < 50) {
                    env.getNodeByID(3).setConcentration(ENABLEDMOL, false);
                } else if (step < 100) {
                    env.getNodeByID(3).setConcentration(ENABLEDMOL, true);
                } else if (step < 10000) { 
                    gradientNode0 = (double) env.getNodeByID(0).getConcentration(DATAMOL);
                    gradientNode1 = (double) env.getNodeByID(1).getConcentration(DATAMOL);
                    gradientNode2 = (double) env.getNodeByID(2).getConcentration(DATAMOL);
                    gradientNode3 = (double) env.getNodeByID(3).getConcentration(DATAMOL);
                    //System.out.println("id0: " + gradientNode0 + " id1: " + gradientNode1 + " id2: " + gradientNode2 + " id3: " + gradientNode3);
                } else {
                    double temp0 = (double) env.getNodeByID(0).getConcentration(DATAMOL);
                    double temp1 = (double) env.getNodeByID(1).getConcentration(DATAMOL);
                    double temp2 = (double) env.getNodeByID(2).getConcentration(DATAMOL);
                    double temp3 = (double) env.getNodeByID(3).getConcentration(DATAMOL);
                    assertTrue("stepDone", temp0 == gradientNode0);
                    assertTrue("stepDone", temp1 == gradientNode1);
                    assertTrue("stepDone", temp2 == gradientNode2);
                    assertTrue("stepDone", temp3 == gradientNode3);
                }
            }
            @Override
            public void initialized(final Environment<Object> env) {
                System.out.println("initialized");
            }
            @Override
            public void finished(final Environment<Object> env, final Time time, final long step) {
                System.out.println("finished"); }
        });

        sim.play();

        while (true) {

        }
    }

}
