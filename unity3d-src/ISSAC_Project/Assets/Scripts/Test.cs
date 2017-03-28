using System;
using UnityEngine;

public class Test : MonoBehaviour 
{
	[SerializeField	]
	private ComUtil com;

	void Start () 
	{
		//NodesDescriptor nd = new NodesDescriptor();
		//nd.AddNode(new Node("1", 10));
		//nd.AddNode(new Node("2", 20));
		//nd.AddNode(new Node("3", 30));

		//com.PostNodes("http://localhost:8080/ ",nd);
		//com.responseArrived += ResponseArrived;
		////com.GetRequest("http://localhost:8080/");
	}

	void Update () 
	{
		
	}




	private void ResponseArrived(string response)
	{
		Debug.Log(response);
	}
}
