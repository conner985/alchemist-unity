using System;
using UnityEngine;

/// <summary>
/// 
/// Class representing an implementation of a Unity version of an Alchemist node, in particular of a node performing a gradient
/// program.
/// 
/// </summary>
[Serializable]
public class GradientNode : ISimNode{

    [SerializeField]
	private int id;
    [SerializeField]
    private NodePosition2D position;
    [SerializeField]
    private GradientMoleculesMap molecules;

    private AbstractBehaviourNode panel;

    public GradientNode(int id, NodePosition2D position, AbstractBehaviourNode panel)
	{
		this.id = id;
        this.position = position;
        this.panel = panel;
        this.molecules = new GradientMoleculesMap();
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

    public GradientMoleculesMap GetMolecules()
    {
        return molecules;
    }

    public void SetMolecule(string mol, object conc)
    {
        molecules.SetMolecule(mol, conc);
    }

    public void SetMolecules(GradientMoleculesMap molecules)
    {
        this.molecules.SetMolecule("data", (double)molecules.GetMoleculeConcentration("data"));
    }

    public AbstractBehaviourNode GetGameObject()
    {
        return panel;
    }
}
