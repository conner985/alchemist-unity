package nodes;

import java.io.Serializable;

public class SingleNode implements Serializable {

	private final int node_id;
	private double data;
	private final NodePosition2D position2d;
	
	public SingleNode(int node_id, double data, NodePosition2D position2d) {
		this.node_id = node_id;
		this.data = data;
		this.position2d = position2d; 
	}
	
	public void moveNode(NodePosition2D position2d){
		position2d.setPosx(position2d.getPosx());
		position2d.setPosy(position2d.getPosy());
	}
	
	public int getID() {
		return node_id;
	}
	
	public NodePosition2D getPosition(){
		return position2d;
	}
	
	public double getData(){
		return data;
	}
	
}
