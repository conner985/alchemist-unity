package nodes;

import it.unibo.alchemist.boundary.interfaces.OutputMonitor;
import it.unibo.alchemist.model.interfaces.Environment;
import it.unibo.alchemist.model.interfaces.Node;
import it.unibo.alchemist.model.interfaces.Reaction;
import it.unibo.alchemist.model.interfaces.Time;

public class CustomMonitor implements OutputMonitor<Object> {

	@Override
	public void stepDone(Environment<Object> env, Reaction<Object> r, Time time, long step) {
		// -> send data to unity
		//System.out.println("stepDone");
	}
	
	@Override
	public void initialized(Environment<Object> env) {
		// -> tell unity we are ready
		System.out.println("initialized");
		env.getSimulation().play();
		System.out.println(env.getSimulation().getStatus());
	}
	
	@Override
	public void finished(Environment<Object> env, Time time, long step) {
		// -> simulation finished
		System.out.println("finished");
	}

}
