package nodes;

import java.util.ArrayList;
import java.util.List;

public class NodesDescriptor {

	private final List<SingleNode> nodes;
	
	public NodesDescriptor() {
		nodes = new ArrayList<SingleNode>();
	}
	
	public void addNode(SingleNode node){
		nodes.add(node);
	}
	
	public List<SingleNode> getNodesList(){
		return nodes;
	}
	
}
