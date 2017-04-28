package nodes;

/***
 * TODO.
 */
public class InitComm {

    private final String type;
    private final int nNodes;
    private final String progType;

    /***
     * @param type TODO
     * @param nNodes TODO
     * @param progType TODO
     */
    public InitComm(final String type, final int nNodes, final String progType) {
        this.type = type;
        this.nNodes = nNodes;
        this.progType = progType;
    }

    /***
     * @return TODO
     */
    public String getType() {
        return type;
    }

    /***
     * @return TODO
     */
    public int getNumNodes() {
        return nNodes;
    }

    /***
     * @return TODO
     */
    public String getProgType() {
        return progType;
    }
}
