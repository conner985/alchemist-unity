using System.Collections.Generic;
using UnityEngine;

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

    public void ClearNodes()
    {
        nodes.Clear();
    }

	public void AddNode(T node)
	{
        nodes.Add(node);
    }

    public void SetType(string type)
    {
        this.type = type;
    }

    public List<T> GetNodes()
    {
        return nodes;
    }

    public IDictionary<int, T> ToDictionary()
    {
        Dictionary<int, T> dict = new Dictionary<int, T>();
        foreach (T simNode in nodes)
        {
            dict.Add(simNode.GetID(), simNode);
        }
        return dict;
    }

    public static string CreateJsonString(NodesDescriptor<T> nd)
    {
        return JsonUtility.ToJson(nd);
    }
    public static NodesDescriptor<T> CreateNodesDescriptorFromJsonString(string json_string)
    {
        return JsonUtility.FromJson<NodesDescriptor<T>>(json_string);
    }

    public string GetCommType()
    {
        return type;
    }
}
