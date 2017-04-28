package nodes;

import java.io.Serializable;

/***
 * TODO.
 */
@SuppressWarnings("serial")
public class GradientNode implements Serializable {

    private static final NodePosition2D DEF_POS = new NodePosition2D(0, 0);
    private final int id;
    private double data;
    private boolean source;
    private boolean enabled;
    private final NodePosition2D position;

    /***
     * @param id TODO
     * @param data TODO
     */
    public GradientNode(final int id, final double data) {
        this(id, data, false, true, DEF_POS);
    }

    /***
     * @param id TODO
     * @param data TODO
     * @param source TODO
     * @param enabled TODO
     * @param position TODO
     */
    public GradientNode(final int id, final double data, final boolean source, final boolean enabled, final NodePosition2D position) {
        this.id = id;
        this.data = data;
        this.position = position; 
        this.source = source;
        this.enabled = enabled;
    }

    /***
     * @param pos TODO
     */
    public void setPosition(final NodePosition2D pos) {
        position.setPosx(pos.getPosx());
        position.setPosz(pos.getPosz());
    }

    /***
     * @return TODO
     */
    public int getID() {
        return id;
    }

    /***
     * @return TODO
     */
    public NodePosition2D getPosition() {
        return position;
    }

    /***
     * @return TODO
     */
    public double getData() {
        return data;
    }

    /***
     * @return TODO
     */
    public boolean isSource() {
        return source;
    }

    /***
     * @return TODO
     */
    public boolean isEnabled() {
        return enabled;
    }

}
