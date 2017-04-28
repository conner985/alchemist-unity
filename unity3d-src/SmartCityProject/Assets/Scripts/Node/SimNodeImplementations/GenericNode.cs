using System;
using UnityEngine;


[Serializable]
public class GenericNode : ISimNode{

    [SerializeField]
	private int id;
    [SerializeField]
    private NodePosition2D position;
    [SerializeField]
    private MoleculesMap molecules = new MoleculesMap();

    public GenericNode(int id, NodePosition2D position)
	{
		this.id = id;
        this.position = position;
    }

    public int GetID()
    {
        return id;
    }

    public NodePosition2D GetPosition()
    {
        return position;
    }

    public void SetPosition(NodePosition2D pos)
    {
        position.SetPosx(pos.GetPosx());
        position.SetPosz(pos.GetPosz());
    }

    public MoleculesMap GetMolecules()
    {
        return molecules;
    }

    public void AddMolecule(string mol, bool conc)
    {
        molecules.AddMolecule(mol, conc);
    }

    public void SetMolecules(MoleculesMap map)
    {
        molecules = map;
    }
}
