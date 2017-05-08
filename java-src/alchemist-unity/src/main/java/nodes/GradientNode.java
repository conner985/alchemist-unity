package nodes;

/***
 * Class that represent a Gradient node.
 * It contains:
 * id: int
 * position: NodePosition2D
 * molecules: GradientMoleculesMap
 */
public class GradientNode {

    private static final NodePosition2D DEF_POS = new NodePosition2D(0, 0);

    private final int id;
    private final NodePosition2D position;
    private final GradientMoleculesMap molecules;


    /***
     * @param id the id you want for this node
     * @param position the position you want for this node
     */
    public GradientNode(final int id, final NodePosition2D position) {
        this.id = id;
        this.position = position;
        molecules = new GradientMoleculesMap();
    }

    /***
     * @param id the id you want for this node
     */
    public GradientNode(final int id) {
        this(id, DEF_POS);
    }

    /***
     * Let you change the position of this node with the one requested.
     * @param pos the new position for this node
     */
    public void setPosition(final NodePosition2D pos) {
        position.setPosx(pos.getPosx());
        position.setPosz(pos.getPosz());
    }

    /***
     * Return the node id.
     * @return the node id
     */
    public int getID() {
        return id;
    }

    /***
     * Return the node position.
     * @return the node position
     */
    public NodePosition2D getPosition() {
        return position;
    }

    /***
     * Return all the molecules of this node.
     * @return the molecules of this node
     */
    public IMoleculesMap getMolecules() {
        return molecules;
    }

    /***
     * Let you set a molecule concentration.
     * @param mol the molecule name
     * @param conc the concentration of that molecule
     */
    public void setMolecule(final String mol, final Object conc) {
        molecules.setMolecule(mol, conc);
    }
}
