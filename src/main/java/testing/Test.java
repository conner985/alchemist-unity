package testing;


import java.util.Collections;

import it.unibo.alchemist.boundary.interfaces.OutputMonitor;
import it.unibo.alchemist.core.implementations.Engine;
import it.unibo.alchemist.core.interfaces.Simulation;
import it.unibo.alchemist.loader.Loader;
import it.unibo.alchemist.loader.YamlLoader;
import it.unibo.alchemist.model.implementations.times.DoubleTime;
import it.unibo.alchemist.model.interfaces.Environment;
import it.unibo.alchemist.model.interfaces.Reaction;
import it.unibo.alchemist.model.interfaces.Time;

public class Test {
	
	public static void main(String[] args) {
		//Alchemist.main(new String[]{"-y","a.yml"});
		String path = "/a.yml";
		final Loader loader = new YamlLoader(Test.class.getResourceAsStream(path));
		System.out.println("loader: "+loader);
		
		final Environment<Object> env = loader.getWith(Collections.emptyMap());
		final Simulation<Object> sim = new Engine<>(env, DoubleTime.INFINITE_TIME);
		
//		sim.play();
		sim.addOutputMonitor(new OutputMonitor<Object>() {
			@Override
			public void stepDone(Environment<Object> env, Reaction<Object> r, Time time, long step) {
				// -> send data to unity
				System.out.println("stepDone");
			}
			
			@Override
			public void initialized(Environment<Object> env) {
				// -> tell unity we are ready
				System.out.println("initialized");
				env.getSimulation().play();
				System.out.println(sim.getStatus());
			}
			
			@Override
			public void finished(Environment<Object> env, Time time, long step) {
				// -> simulation finished
				System.out.println("finished");
			}
		});
//		new Thread(() -> {
//			while(true) {
//				// read data from unity
//				sim.schedule(() -> {
//					env.moveNode(env.getNodeByID(0), env.makePosition(2, 3));
//				});
//			}
//		});
		new Thread(sim).start();

	}
}
