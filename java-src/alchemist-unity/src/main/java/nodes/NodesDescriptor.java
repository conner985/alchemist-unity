package nodes;

import java.util.ArrayList;
import java.util.List;

/***
 * Class that contains a list of all nodes.
 * Used primarily to deserialize and serialize automatically using Json utilities
 * @param <T> the type of the nodes
 */
public class NodesDescriptor<T extends GradientNode> {

    private final List<T> nodes = new ArrayList<>();

    /***
     * Let you add a node to the list.
     * @param node the node you want to add
     */
    public void addNode(final T node) {
        nodes.add(node);
    }

    /***
     * Return a list containing all the nodes.
     * @return the node list
     */
    public List<T> getNodesList() {
        return nodes;
    }

    /***
     * Let you remove all the nodes from the list.
     */
    public void clear() {
        nodes.clear();
    }

}
