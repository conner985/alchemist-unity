using UnityEngine;
using System;

public class ExitBehaviour : AbstractBehaviourNode
{
    [SerializeField]
    private bool isSource = true;

    private GradientNode node;

    public void ReceiveInfo(object info)
    {
    }

    public override void CreateNode(int id, SimNodeTypes.type simNodeType)
    {
        if (simNodeType.Equals(SimNodeTypes.type.GRADIENT))
        {
            node = new GradientNode(id, new NodePosition2D(transform.position.x, transform.position.z), this);
            node.SetMolecule("source", isSource);
            node.SetMolecule("enabled", enabled);
        }
    }

    public override GradientNode GetNode()
    {
        node.SetPosition(new NodePosition2D(transform.position.x, transform.position.z));
        return node;
    }
}
