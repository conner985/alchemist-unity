using System.Collections.Generic;

public interface ISimNode
{
    NodePosition2D GetPosition();
    void SetPosition(NodePosition2D pos);

    int GetID();

    MoleculesMap GetMolecules();
    void AddMolecule(string mol, bool conc);
}
