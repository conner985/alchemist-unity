package nodes;

/***
 * This interface defines a structure for a generic class that you can use for serialize and deserialize, for example, a Json message.
 */
public interface IMoleculesMap {

    /***
     * Return the concentration of a given molecule name.
     * @param mol the name of the molecule
     * @return the concentration of the molecule
     */
    Object getMoleculeConcentration(String mol);

    /***
     * Set the internal molecule of this map based on the one passed.
     * @param molecules an instance of another IMoleculeMap you want to copy
     */
    void setMolecules(IMoleculesMap molecules);

    /***
     * Given a molecule name and its concentration, it will set the concentration for the molecule with that name present in this class.
     * @param mol the name of the molecule you want to set
     * @param conc the concentration of the molecule you want to set
     */
    void setMolecule(String mol, Object conc);

}
