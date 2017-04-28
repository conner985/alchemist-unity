package nodes;

import java.io.Serializable;
/***
 * TODO.
 */
@SuppressWarnings("serial")
public class NodePosition2D implements Serializable {

    private double posx, posz;

    /***
     * @param posx TODO
     * @param posz TODO
     */
    public NodePosition2D(final double posx, final double posz) {
        this.posx = posx;
        this.posz = posz;
    }

    /***
     * @return TODO
     */
    public double getPosx() {
        return posx;
    }

    /***
     * @param posx TODO
     */
    public void setPosx(final double posx) {
        this.posx = posx;
    }
    /***
     * @return TODO
     */
    public double getPosz() {
        return posz;
    }

    /***
     * @param posz TODO
     */
    public void setPosz(final double posz) {
        this.posz = posz;
    }

}
