package nodes;

/***
 * TODO.
 */
public class GradientNode {

    private static final NodePosition2D DEF_POS = new NodePosition2D(0, 0);

    private final int id;
    private final NodePosition2D position;
    private final GradientMoleculesMap molecules;


    /***
     * @param id TODO
     * @param position TODO
     */
    public GradientNode(final int id, final NodePosition2D position) {
        this.id = id;
        this.position = position;
        molecules = new GradientMoleculesMap();
    }

    /***
     * @param id TODO
     */
    public GradientNode(final int id) {
        this(id, DEF_POS);
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
    public IMoleculesMap getMolecules() {
        return molecules;
    }

    /***
     * @param mol TODO
     * @param conc TODO
     */
    public void setMolecule(final String mol, final Object conc) {
        molecules.setMolecule(mol, conc);
    }
}
