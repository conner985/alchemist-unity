using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Class containing a collection of all nodes to be exchanged with Alchemist and the communication type (init or step)
/// 
/// </summary>
/// <typeparam name="T">The type of the nodes (CURRENTLY ONLY GradientNode SUPPORTED)</typeparam>
[System.Serializable]
public class NodesDescriptor<T> where T : ISimNode
{
	[SerializeField]
	private List<T> nodes;
    [SerializeField]
    private string type;

	public NodesDescriptor()
	{
		nodes = new List<T>();
	}

    /// <summary>
    /// 
    /// Clears the list
    /// 
    /// </summary>
    public void ClearNodes()
    {
        nodes.Clear();
    }

    /// <summary>
    /// 
    /// Adds a node to the list of nodes
    /// 
    /// </summary>
    /// <param name="node">The node to be added</param>
	public void AddNode(T node)
	{
        nodes.Add(node);
    }

    /// <summary>
    /// 
    /// Sets the type of the NodeDescriptor (used to discriminate the communication type, init or step mode)
    /// 
    /// </summary>
    /// <param name="type">Type of the communication (CURRENTLY COULD BE init OR step, please see the general documentation)</param>
    public void SetType(string type)
    {
        this.type = type;
    }

    /// <summary>
    /// Get all nodes
    /// </summary>
    /// <returns>List of all nodes</returns>
    public List<T> GetNodes()
    {
        return nodes;
    }

    /// <summary>
    /// It builds a dictionary with ID as key and the corrispondent node as a value
    /// </summary>
    /// <returns>A dictionary of IDs/nodes</returns>
    public IDictionary<int, T> ToDictionary()
    {
        Dictionary<int, T> dict = new Dictionary<int, T>();
        foreach (T simNode in nodes)
        {
            dict.Add(simNode.GetID(), simNode);
        }
        return dict;
    }

    /// <summary>
    /// 
    /// It creates a Json string from a NodesDescriptor ready to be sent to Alchemist
    /// 
    /// </summary>
    /// <param name="nd">The NodesDescriptor to be serialized into a Json string</param>
    /// <returns>The Json string</returns>
    public static string CreateJsonString(NodesDescriptor<T> nd)
    {
        return JsonUtility.ToJson(nd);
    }

    /// <summary>
    /// 
    /// It creates a NodesDescriptor object from a given Json string
    /// 
    /// </summary>
    /// <param name="json_string">The Json string to be deserialized</param>
    /// <returns>The NodesDescriptor object</returns>
    public static NodesDescriptor<T> CreateNodesDescriptorFromJsonString(string json_string)
    {
        return JsonUtility.FromJson<NodesDescriptor<T>>(json_string);
    }

    /// <summary>
    /// Returns the communication type
    /// </summary>
    /// <returns>The communication type</returns>
    public string GetCommType()
    {
        return type;
    }
}
