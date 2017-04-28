package nodes;

import java.io.Serializable;
import java.util.HashMap;

/***
 * TODO.
 */
@SuppressWarnings("serial")
public class UnityGenericNode implements Serializable {

    private static final NodePosition2D DEF_POS = new NodePosition2D(0, 0);

    private final int id;
    private final NodePosition2D position;
    private final MoleculesMap molecules;


    /***
     * @param id TODO
     * @param position TODO
     */
    public UnityGenericNode(final int id, final NodePosition2D position) {
        this.id = id;
        this.position = position;
        molecules = new MoleculesMap();
    }

    /***
     * @param id TODO
     */
    public UnityGenericNode(final int id) {
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
    public HashMap<String, Object> getMolecules() {
        return molecules.getMolecules();
    }

    /***
     * @param mol TODO
     * @param conc TODO
     */
    public void addMolecule(final String mol, final Object conc) {
        molecules.addMolecule(mol, conc);
    }
}
