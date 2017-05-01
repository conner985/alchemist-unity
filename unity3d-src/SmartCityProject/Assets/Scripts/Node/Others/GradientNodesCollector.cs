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

    private NodesDescriptor<GradientNode> nd;
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
        nd = com.RequestGET_Sync<GradientNode>("http://localhost:8080/");
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

        else if (stopWatchGET.Elapsed.TotalMilliseconds > get2postTime)
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
        com.responseArrivedGET -= ResponseArrivedGET;
        UnityEngine.Debug.Log("GET ASYNC: " + response);

        NodesDescriptor<GradientNode> newND = NodesDescriptor<GradientNode>.CreateNodesDescriptorFromJsonString(response);
        UpdateAllNodes(newND);
        stopWatchPOST.Start();
    }

    private void ResponseArrivedPOST(string response)
    {
        com.responseArrivedPOST -= ResponseArrivedPOST;
        stopWatchGET.Start();
    }

    private void CollectAllNodes()
    {
        //if (rnd.NextDouble() < 0.1f)
        behaviourNodes = FindObjectsOfType(typeof(AbstractBehaviourNode)) as IList<AbstractBehaviourNode>;

        nd.ClearNodes();
        nd.SetType("step");
        foreach(AbstractBehaviourNode bn in behaviourNodes)
        {
            nd.AddNode(bn.GetNode());
        }
    }

    private void UpdateAllNodes(NodesDescriptor<GradientNode> newND)
    {
        CollectAllNodes();
        IDictionary<int, GradientNode> newDict = newND.ToDictionary();
        int i = 0;
        for(i = 0; i < nd.GetNodes().Count; i++)
        {
            GradientNode newSimNode;
            var node = nd.GetNodes()[i];
            if (newDict.TryGetValue(node.GetID(), out newSimNode))
            {
                var molecules = newSimNode.GetMolecules();
                node.SetMolecules(molecules);
            }
        }

    }
}
