public interface IMoleculesMap
{
    void SetMolecule(string mol, object conc);
    object GetMoleculeConcentration(string mol);
    void SetMolecules(IMoleculesMap molecules);
}
