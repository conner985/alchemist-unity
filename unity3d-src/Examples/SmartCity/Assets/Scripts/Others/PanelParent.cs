using UnityEngine;
using System.Collections;

public class PanelParent : MonoBehaviour { 

    void Awake()
    {
        GetComponent<SphereCollider>().enabled = false;
    }

	// Use this for initialization
	void Start () { 

    }
	
	// Update is called once per frame
	void Update () {

    }
}
