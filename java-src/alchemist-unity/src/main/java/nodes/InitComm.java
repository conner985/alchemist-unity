package nodes;

/***
 * Class used to automatically deserialize, through Gson utility, a Json message that unity will send as initialization.
 */
public class InitComm {

    private final String type;
    private final int nNodes;
    private final String progType;

    /***
     * @param type the type of the communication (e.g. "init")
     * @param nNodes the number of nodes you want in your simulation (useless for now since the cloning it's bugged)
     * @param progType the program you want in your nodes (e.g. GRADIENT)
     */
    public InitComm(final String type, final int nNodes, final String progType) {
        this.type = type;
        this.nNodes = nNodes;
        this.progType = progType;
    }

    /***
     * Return the type of this communication.
     * @return the type of the communication
     */
    public String getType() {
        return type;
    }

    /***
     * Return the number of nodes requested for this communication.
     * @return the number of nodes requested
     */
    public int getNumNodes() {
        return nNodes;
    }

    /***
     * Return the type of the program that has ben requested.
     * @return the name of the program requested
     */
    public String getProgType() {
        return progType;
    }
}
