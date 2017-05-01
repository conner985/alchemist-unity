public abstract class AbstractBehaviourNode : UnityEngine.MonoBehaviour
{
    public abstract GradientNode GetNode();
    public abstract void CreateNode(int id, SimNodeTypes.type simNodeType);
}
