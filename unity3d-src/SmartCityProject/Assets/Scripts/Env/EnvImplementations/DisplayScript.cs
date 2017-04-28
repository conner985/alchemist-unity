using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DisplayScript : MonoBehaviour {

    public Material left_arrow, right_arrow, up_arrow, down_arrow;
    
    private Renderer ren;
    private Material org_mat;

    void Awake()
    { 
        ren = GetComponent<Renderer>();
        org_mat = ren.material;
    }

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void RenderArrow(Vector3 closest)
    {
        Vector3 vector = transform.parent.transform.InverseTransformPoint(closest);

        if(Mathf.Abs(vector.z) >= Mathf.Abs(vector.x))
        {
            if (vector.z <= 0) ren.material = left_arrow;
            else ren.material = right_arrow;
        }
        else
        {
            if (vector.x <= 0) ren.material = up_arrow;
            else ren.material = down_arrow;
        }

        
    }

    internal void Clear()
    {
        ren.material = org_mat;
    }
}
