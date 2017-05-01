package nodes;

import java.util.ArrayList;
import java.util.List;

/***
 * TODO.
 * @param <T>
 */
public class NodesDescriptor<T extends GradientNode> {

    private final List<T> nodes = new ArrayList<>();

    /***
     * @param node TODO
     */
    public void addNode(final T node) {
        nodes.add(node);
    }

    /***
     * @return TODO
     */
    public List<T> getNodesList() {
        return nodes;
    }

    /***
     * TODO.
     */
    public void clear() {
        nodes.clear();
    }

}
