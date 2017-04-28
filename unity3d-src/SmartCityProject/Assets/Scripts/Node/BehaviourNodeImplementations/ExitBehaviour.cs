using UnityEngine;
using System;

public class ExitBehaviour : AbstractBehaviourNode
{
    [SerializeField]
    private bool isSource = true;

    private GenericNode node;

    public void ReceiveInfo(object info)
    {
    }

    public override void CreateNode(int id, SimNodeTypes.type simNodeType)
    {
        if (simNodeType.Equals(SimNodeTypes.type.GRADIENT))
        {
            node = new GenericNode(id, new NodePosition2D(transform.position.x, transform.position.z));
            node.AddMolecule("source", isSource);
            node.AddMolecule("enabled", enabled);
        }
    }

    public override GenericNode GetNode()
    {
        node.SetPosition(new NodePosition2D(transform.position.x, transform.position.z));
        return node;
    }
}
