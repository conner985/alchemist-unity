/// <summary>
/// 
/// Abstract class representing a node that lives within Unity, it will contain an ISimNode (GradientNode in this project)
/// representing the Unity version of the Alchemist node.
/// 
/// </summary>
public abstract class AbstractBehaviourNode : UnityEngine.MonoBehaviour
{
    /// <summary>
    /// 
    /// Method that will return the GradientNode of this node
    /// 
    /// </summary>
    /// <returns>The GradientNode associated to this node</returns>
    public abstract GradientNode GetNode();

    /// <summary>
    /// 
    /// Method that creates a new GradientNode within this node, it will be collected and sent to Alchemist
    /// 
    /// </summary>
    /// <param name="id">The ID of the GradientNode</param>
    /// <param name="simNodeType">The type of the node to create (CURRENTLY ONLY 'GRADIENT' SUPPORTED)</param>
    public abstract void CreateNode(int id, SimNodeTypes.type simNodeType);
}
