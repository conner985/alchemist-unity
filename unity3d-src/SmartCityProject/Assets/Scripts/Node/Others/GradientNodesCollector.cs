using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using System;

public class GradientNodesCollector : MonoBehaviour {

    [SerializeField]
    private ComUtil com;
    [SerializeField]
    private int post2getTime = 1000;
    [SerializeField]
    private int get2postTime = 500;

    private Stopwatch stopWatchGET;
    private Stopwatch stopWatchPOST;

    private NodesDescriptor<GenericNode> nd;
    private IList<AbstractBehaviourNode> behaviourNodes;

    private System.Random rnd;
    private bool doComm;

    void Start ()
    {
        Application.runInBackground = true;
        // pause the simulation
        Time.timeScale = 0;

        rnd = new System.Random(DateTime.Now.Millisecond);

        behaviourNodes = FindObjectsOfType(typeof(AbstractBehaviourNode)) as IList<AbstractBehaviourNode>;

        stopWatchGET = Stopwatch.StartNew();
        stopWatchGET.Reset();
        stopWatchPOST = Stopwatch.StartNew();
        stopWatchPOST.Reset(); 

        com.RequestPOSTNodes_Sync("http://localhost:8080/", behaviourNodes.Count, "","GRADIENT");
        nd = com.RequestGET_Sync<GenericNode>("http://localhost:8080/");
        CreateAllNodes();

        // start the simulation
        Time.timeScale = 1;
        stopWatchPOST.Start();
    }

    void Update ()
    {
        //if (!doComm) return;
        if (stopWatchPOST.Elapsed.TotalMilliseconds > post2getTime)
        {
            stopWatchPOST.Reset();
            CollectAllNodes();
            SendNodes();
        }

        if (stopWatchGET.Elapsed.TotalMilliseconds > get2postTime)
        {
            stopWatchGET.Reset();
            ReceiveNodes();
        }
    }

    private void CreateAllNodes()
    {
        if (nd.GetNodes().Count != behaviourNodes.Count)
        {
            //errore!!!
            UnityEngine.Debug.Log("unity nodes: " + behaviourNodes.Count);
            UnityEngine.Debug.Log("alchemist nodes: " + nd.GetNodes().Count);
            UnityEngine.Debug.LogError("Inconsistent number of nodes between Unity and Alchemist");
            return;
        }

        int i;
        for (i = 0; i < behaviourNodes.Count; i++)
        {
            int id = nd.GetNodes()[i].GetID();
            behaviourNodes[i].CreateNode(id, SimNodeTypes.type.GRADIENT);
        }

    }

    private void SendNodes()
    {
        UnityEngine.Debug.Log("keys 0: " + nd.GetNodes()[0].GetMolecules().GetConcentration("enabled"));

        com.RequestPOSTNodes_Async("http://localhost:8080/ ", nd, "");
        com.responseArrivedPOST += ResponseArrivedPOST;
    }

    private void ReceiveNodes()
    {

        com.RequestGET_Async("http://localhost:8080/ ");
        com.responseArrivedGET += ResponseArrivedGET;
    }

    private void ResponseArrivedGET(string response)
    {

        UnityEngine.Debug.Log("GET ASYNC: " + response);

        NodesDescriptor<GenericNode> newND = NodesDescriptor<GenericNode>.CreateNodesDescriptorFromJsonString(response);
        UpdateAllNodes(newND);
        stopWatchPOST.Start();
    }

    private void ResponseArrivedPOST(string response)
    {
        stopWatchGET.Start();
    }

    private void CollectAllNodes()
    {
        if (rnd.NextDouble() < 0.1f) behaviourNodes = FindObjectsOfType(typeof(AbstractBehaviourNode)) as IList<AbstractBehaviourNode>;

        nd.ClearNodes();
        nd.SetType("step");
        foreach(AbstractBehaviourNode bn in behaviourNodes)
        {
            nd.AddNode(bn.GetNode());
        }
    }

    private void UpdateAllNodes(NodesDescriptor<GenericNode> newND)
    {
        CollectAllNodes();
        IDictionary<int, GenericNode> newDict = newND.ToDictionary();
        foreach (GenericNode oldSimNode in nd.GetNodes())
        {
            GenericNode newSimNode;
            if (newDict.TryGetValue(oldSimNode.GetID(), out newSimNode))
            {
                var molecules = newSimNode.GetMolecules();
                oldSimNode.SetMolecules(molecules);
            }
        }

    }
}
