using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using System;

/// <summary>
/// 
/// Class used as a communication centre: it will make an initial setup using the synchronous POST (communicating the number of nodes
/// and the program type to Alchemist) followed by a GET (retrieving the IDs of the nodes) and the creation of a GradientNode (the 
/// Unity version of the Alchemist node) per AbstractBehaviourNode (a node that lives within Unity), then it will periodically make a
/// nodes gathering, sending (asynch POST) all GradientNodes with their new positions to Alchemist and retrieving (asynch GET) all 
/// molecules updates
/// 
/// </summary>
public class GradientNodesCollector : MonoBehaviour
{

    /// <summary>
    /// The ComUtil for the communication
    /// </summary>
    [SerializeField]
    private ComUtil com;
    /// <summary>
    /// Time between a POST and a GET request
    /// </summary>
    [SerializeField]
    private int post2getTime = 1000;
    /// <summary>
    /// Time between a GET and a POST request
    /// </summary>
    [SerializeField]
    private int get2postTime = 500;

    private Stopwatch stopWatchGET;
    private Stopwatch stopWatchPOST;

    private NodesDescriptor<GradientNode> nd;
    private IList<AbstractBehaviourNode> behaviourNodes;

    private System.Random rnd;
    private bool doComm;

    /// <summary>
    /// 
    /// It will perform the setup inizialization phase
    /// 
    /// </summary>
    void Start()
    {
        Application.runInBackground = true;
        // pause the simulation
        Time.timeScale = 0;

        rnd = new System.Random(DateTime.Now.Millisecond);

        //retrieving of all AbstractBehaviourNode in the scene
        behaviourNodes = FindObjectsOfType(typeof(AbstractBehaviourNode)) as IList<AbstractBehaviourNode>;

        stopWatchGET = Stopwatch.StartNew();
        stopWatchGET.Reset();
        stopWatchPOST = Stopwatch.StartNew();
        stopWatchPOST.Reset();

        //SETUP START
        com.RequestPOSTNodes_Sync("http://localhost:8080/", behaviourNodes.Count, "", "GRADIENT");
        nd = com.RequestGET_Sync<GradientNode>("http://localhost:8080/");
        CreateAllNodes();
        //SETUP END

        // start the simulation
        Time.timeScale = 1;
        stopWatchPOST.Start();
    }

    /// <summary>
    /// 
    /// It wil periodically do the gathering, sending and updating phases
    /// 
    /// </summary>
    void Update()
    {
        if (stopWatchPOST.Elapsed.TotalMilliseconds > post2getTime)
        {
            stopWatchPOST.Reset();
            //gathering of all GradientNodes in the scene
            CollectAllNodes();
            //sending of all nodes
            SendNodes();
        }

        else if (stopWatchGET.Elapsed.TotalMilliseconds > get2postTime)
        {
            stopWatchGET.Reset();
            //retrieving all updates from Alchemist
            ReceiveNodes();
        }
    }

    private void CreateAllNodes()
    {
        if (nd.GetNodes().Count != behaviourNodes.Count)
        {

            //errore!!!
            UnityEngine.Debug.LogError("Unity nodes: " + behaviourNodes.Count);
            UnityEngine.Debug.LogError("Alchemist nodes: " + nd.GetNodes().Count);
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

        com.RequestGET_Async("http://localhost:8080/");
        com.responseArrivedGET += ResponseArrivedGET;
    }

    /// <summary>
    /// 
    /// Delegate of the asynchronous GET: it will deserialize the response Json string into a proper NodesDescriptor object
    /// and it will update all nodes
    /// 
    /// </summary>
    /// <param name="response">The response to the request</param>
    private void ResponseArrivedGET(string response)
    {
        com.responseArrivedGET -= ResponseArrivedGET;

        NodesDescriptor<GradientNode> newND = NodesDescriptor<GradientNode>.CreateNodesDescriptorFromJsonString(response);
        UpdateAllNodes(newND);
        stopWatchPOST.Start();
    }

    /// <summary>
    /// 
    /// Delegate of the asynchronous POST request
    /// 
    /// </summary>
    /// <param name="response">The response to the request</param>
    private void ResponseArrivedPOST(string response)
    {
        com.responseArrivedPOST -= ResponseArrivedPOST;
        stopWatchGET.Start();
    }

    private void CollectAllNodes()
    {
        if (rnd.NextDouble() < 0.1f)
        {
            behaviourNodes = FindObjectsOfType(typeof(AbstractBehaviourNode)) as IList<AbstractBehaviourNode>;
        }

        nd.ClearNodes();
        nd.SetType("step");
        foreach (AbstractBehaviourNode bn in behaviourNodes)
        {
            nd.AddNode(bn.GetNode());
        }
    }

    private void UpdateAllNodes(NodesDescriptor<GradientNode> newND)
    {
        CollectAllNodes();
        IDictionary<int, GradientNode> newDict = newND.ToDictionary();
        int i = 0;
        for (i = 0; i < nd.GetNodes().Count; i++)
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
