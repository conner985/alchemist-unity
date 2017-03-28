using UnityEngine;
using System.Diagnostics;

public class NodesCollector : MonoBehaviour {

    [SerializeField]
    private ComUtil com;

    private Stopwatch stopWatchGET;
    private Stopwatch stopWatchPOST;

    private PersonBehaviour[] people;

    private int id_counter;

    void Awake()
    {
        id_counter = 0;

        people = FindObjectsOfType<PersonBehaviour>();
    }

    void Start ()
    {
        stopWatchGET = Stopwatch.StartNew();
        stopWatchGET.Reset();
        stopWatchPOST = Stopwatch.StartNew();
        com.RequestPOSTNodes_Sync("http://localhost:8080/ ", 4, "");
        NodesDescriptor ns = com.RequestGET_Sync("http://localhost:8080/ ");
        UnityEngine.Debug.Log(ns);
    }
	
	void Update () {
        //if(stopWatchPOST.Elapsed.TotalMilliseconds > 1000)
        //{
        //    stopWatchPOST.Reset();
        //    NodesDescriptor nodes = CollectAllNodes();
        //    UnityEngine.Debug.Log("COLLECTING NODES");
        //    SendNodes(nodes);
        //}

        //if(stopWatchGET.Elapsed.TotalMilliseconds > 1000)
        //{
        //    stopWatchGET.Reset();
        //    ReceiveNodes();
        //}
        

    }

    private void SendNodes(NodesDescriptor nodes)
    {
        com.RequestPOSTNodes_Async("http://localhost:8080/ ", nodes, "");
        com.responseArrivedPOST += ResponseArrivedPOST;
    }

    private void ReceiveNodes()
    {

        com.RequestGET_Async("http://localhost:8080/ ");
        com.responseArrivedGET += ResponseArrivedGET;
    }

    private void ResponseArrivedGET(string response)
    {
        //decodificare
        //aggiornare i nodi delle persone
        stopWatchPOST.Start();
    }

    private void ResponseArrivedPOST(string response)
    {
        stopWatchGET.Start();
    }

    private NodesDescriptor CollectAllNodes()
    {
        PersonBehaviour[] people = FindObjectsOfType<PersonBehaviour>();
        NodesDescriptor ns = new NodesDescriptor();
        ns.ClearNodes();
        ns.SetType("init");
        foreach(PersonBehaviour person in people)
        {
            ns.AddNode(person.GetNode());
        }
        return ns;
    }

    public int GenerateNewID()
    {
        return id_counter++;
    } 
}
