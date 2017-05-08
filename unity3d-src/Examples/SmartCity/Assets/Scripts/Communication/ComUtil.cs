using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Threading;

/// <summary>
/// 
/// Utility useful for communication purpose.
/// 
/// </summary>
public class ComUtil : MonoBehaviour
{
    /// <summary>
    /// 
    /// The delegate function called by the event triggered by a GET asynchronous request
    /// 
    /// </summary>
    /// <param name="response">The GET string received</param>
    public delegate void ResponseArrivedGET(string response);
    /// <summary>
    /// 
    /// The event triggered when a GET asynchronous response arrives, it is responsable to call each registered delegate function
    /// 
    /// </summary>
    public event ResponseArrivedGET responseArrivedGET;

    /// <summary>
    /// 
    /// The delegate function called by the event triggered by a POST asynchronous request
    /// 
    /// </summary>
    /// <param name="response">The POST string response, works as an request check</param>
    public delegate void ResponseArrivedPOST(string response);
    /// <summary>
    /// 
    /// The event triggered when a POST asynchronous response arrives, it is responsable to call each registered delegate function
    /// 
    /// </summary>
    public event ResponseArrivedPOST responseArrivedPOST;


    #region ASYNC COMM
    /// <summary>
    /// 
    /// Generic asynchronous GET request: in order to have an asynchronous response, a delegate must be registered 
    /// and the response will be sent to everyone registered to the event 'responseArrivedGET'
    /// 
    /// </summary>
    /// <param name="url">The url of the server</param>
    public void RequestGET_Async(string url)
	{
		UnityWebRequest req = UnityWebRequest.Get(url);
		StartCoroutine(SendGETRequestAndWait(req));
	}

    /// <summary>
    /// 
    /// Asynchronous POST request: it will make an asynchronous POST request of a given NodesDescriptor object encoded in a 
    /// proper Json string using the delegate 'responseArrivedPOST' to handle the response.
    /// Used foremost to send all nodes position updates to Alchemist
    /// 
    /// </summary>
    /// <typeparam name="T">Type of nodes to be collected</typeparam>
    /// <param name="url">The url of the server</param>
    /// <param name="nodes">The NodesDescriptor containing all nodes in the Unity scene</param>
    /// <param name="postData">Not used</param>
	public void RequestPOSTNodes_Async<T>(string url, NodesDescriptor<T> nodes, string postData) where T : ISimNode
    {
		string json_string = NodesDescriptor<T>.CreateJsonString(nodes);

		UnityWebRequest req = UnityWebRequest.Post(url,postData);

		byte[] json_bytes = System.Text.Encoding.UTF8.GetBytes(json_string);

		UploadHandlerRaw uphandler = new UploadHandlerRaw(json_bytes);
		uphandler.contentType = "application/json";

		req.uploadHandler = uphandler;
		
		StartCoroutine(SendPOSTRequestAndWait(req));
    }
    
    private IEnumerator SendGETRequestAndWait(UnityWebRequest req)
    {
        yield return req.Send();
        responseArrivedGET(req.downloadHandler.text);
    }
    
    private IEnumerator SendPOSTRequestAndWait(UnityWebRequest req)
    {
        yield return req.Send();
        responseArrivedPOST(req.downloadHandler.text);
    }

    #endregion

    #region SYNC COMM

    /// <summary>
    /// 
    /// Synchronous GET request, used during the inizialization phase: it will retrieve a NodesDescriptor object containing
    /// all nodes with relative molecules updates
    /// 
    /// </summary>
    /// <typeparam name="T">Types of the nodes</typeparam>
    /// <param name="url">The url of the server</param>
    /// <returns>The NodesDescriptor containing all updated nodes</returns>
    public NodesDescriptor<T> RequestGET_Sync<T>(string url) where T : ISimNode
    {
        UnityWebRequest req = UnityWebRequest.Get(url);

        AsyncOperation op = req.Send();
        while (!op.isDone) { Thread.Sleep(100); }

        return JsonUtility.FromJson<NodesDescriptor<T>>(req.downloadHandler.text);
    }

    /// <summary>
    /// 
    /// Synchronous POST request, used during the inizialization phase: it will make a request sending a Json string
    /// containing the number of nodes to create and the progType to load (implemented in the InitComm object)
    /// 
    /// </summary>
    /// <param name="url">The url of the server called</param>
    /// <param name="n_nodes">The number of nodes to create</param>
    /// <param name="postData">Not used</param>
    /// <param name="progType">The type of the program (CURRENTLY ONLY GRADIENT SUPPORTED)</param>
    /// <returns></returns>
    public string RequestPOSTNodes_Sync(string url, int n_nodes, string postData, string progType)
    {
        InitComm initComm = new InitComm("init", n_nodes, progType);
        string json_string = JsonUtility.ToJson(initComm);

        UnityWebRequest req = UnityWebRequest.Post(url, postData);

        byte[] json_bytes = System.Text.Encoding.UTF8.GetBytes(json_string);

        UploadHandlerRaw uphandler = new UploadHandlerRaw(json_bytes);
        uphandler.contentType = "application/json";

        req.uploadHandler = uphandler;

        AsyncOperation op = req.Send();
        while (!op.isDone) { Thread.Sleep(100); }

        return req.downloadHandler.text;
    }

    #endregion
}
