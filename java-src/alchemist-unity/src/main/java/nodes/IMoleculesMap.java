package nodes;

/***
 * TODO.
 *
 */
public interface IMoleculesMap {

    /***
     * @param mol TODO
     * @return TODO
     */
    Object getMoleculeConcentration(String mol);

    /***
     * @param molecules TODO
     */
    void setMolecules(IMoleculesMap molecules);

    /***
     * @param mol TODO
     * @param conc TODO
     */
    void setMolecule(String mol, Object conc);

}
