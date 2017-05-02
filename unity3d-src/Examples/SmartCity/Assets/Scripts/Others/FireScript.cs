using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

    public Material activeMat;
    public Material idleMat;

    private Renderer ren;
    private ParticleSystem particleSys;
    private bool isActive;

    void Awake()
    {
        ren = GetComponent<Renderer>();
        ren.material = idleMat;
        particleSys = GetComponentInChildren<ParticleSystem>();
    }
    
    void Start () { }
	void Update () { }

    void OnMouseDown()
    {
        if (isActive)
        {
            isActive = false;
            ren.material = idleMat;
            particleSys.Stop();
        }
        else
        {
            isActive = true;
            ren.material = activeMat;
            particleSys.Play();
        }
    }
}
