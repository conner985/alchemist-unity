public interface ISimNode
{
    NodePosition2D GetPosition();
    void SetPosition(NodePosition2D pos);

    int GetID();

    GradientMoleculesMap GetMolecules();
    void SetMolecules(GradientMoleculesMap molecules);

    void SetMolecule(string mol, object conc);
}
