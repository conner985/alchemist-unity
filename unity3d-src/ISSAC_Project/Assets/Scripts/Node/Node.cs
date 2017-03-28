using UnityEngine;

[System.Serializable]
public class Node
{
	[SerializeField]
	private int id;
    [SerializeField]
    private float data;
    [SerializeField]
    private NodePosition2D position;

    public Node(int id, float data, NodePosition2D position)
	{
		this.id = id;
		this.data = data;
        this.position = position;
	}

	public int GetId()
	{
		return id;
	}

	public float GetData()
	{
		return data;
	}

    public NodePosition2D GetPostion()
    {
        return position;
    }

    public void SetPosition(NodePosition2D pos)
    {
        position.SetPosx(pos.GetPosx());
        position.SetPosz(pos.GetPosz());
    }
}
