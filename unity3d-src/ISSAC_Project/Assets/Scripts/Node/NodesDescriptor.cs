using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodesDescriptor
{
	[SerializeField]
	private List<Node> nodes;
    [SerializeField]
    private string type;

	public NodesDescriptor()
	{
		nodes = new List<Node>();
	}

    public void ClearNodes()
    {
        nodes.Clear();
    }

	public void AddNode(Node node)
	{
        nodes.Add(node);
	}

	public static string CreateJsonString(NodesDescriptor nd)
	{
		return JsonUtility.ToJson(nd);
	}

    public void SetType(string type)
    {
        this.type = type;
    }
}
