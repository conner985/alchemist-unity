package nodes;

import java.io.Serializable;
/***
 * Class that represent the 2D position information of a node: x,z .
 */
@SuppressWarnings("serial")
public class NodePosition2D implements Serializable {

    private double posx, posz;

    /***
     * Class constructor that let you create a position for a node.
     * @param posx the x position of the node
     * @param posz the z position of the node
     */
    public NodePosition2D(final double posx, final double posz) {
        this.posx = posx;
        this.posz = posz;
    }

    /***
     * Return the x position of the node.
     * @return the x position of the node
     */
    public double getPosx() {
        return posx;
    }

    /***
     * Set the x position of the node.
     * @param posx the x position of the node
     */
    public void setPosx(final double posx) {
        this.posx = posx;
    }
    /***
     * Return the x position of the node.
     * @return the z position of the node
     */
    public double getPosz() {
        return posz;
    }

    /***
     * Set the x position of the node.
     * @param posz the z position of the node
     */
    public void setPosz(final double posz) {
        this.posz = posz;
    }

}
