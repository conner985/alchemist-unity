public abstract class AbstractBehaviourNode : UnityEngine.MonoBehaviour
{
    public abstract GenericNode GetNode();
    public abstract void CreateNode(int id, SimNodeTypes.type simNodeType);
}
