using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Threading;

public class ComUtil : MonoBehaviour
{
    public delegate void ResponseArrivedGET(string response);
    public event ResponseArrivedGET responseArrivedGET;

    public delegate void ResponseArrivedPOST(string response);
    public event ResponseArrivedPOST responseArrivedPOST;

    
    #region ASYNC COMM

    public void RequestGET_Async(string url)
	{
		UnityWebRequest req = UnityWebRequest.Get(url);
		StartCoroutine(SendGETRequestAndWait(req));
	}

	public void RequestPOSTNodes_Async<T>(string url, NodesDescriptor<T> nodes, string postData) where T : ISimNode
    {
		string json_string = NodesDescriptor<T>.CreateJsonString(nodes);

        Debug.Log("POST ASYNC: " + json_string);

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

    public NodesDescriptor<T> RequestGET_Sync<T>(string url) where T : ISimNode
    {
        UnityWebRequest req = UnityWebRequest.Get(url);

        AsyncOperation op = req.Send();
        while (!op.isDone) { Thread.Sleep(100); }

        Debug.Log("GET SYNC: " + req.downloadHandler.text);

        return JsonUtility.FromJson<NodesDescriptor<T>>(req.downloadHandler.text);
    }

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
