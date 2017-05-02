using UnityEngine; 
using System;

public class SmokeSensor : MonoBehaviour,ISmokeSensor {
    
    private bool alarm;
    private Renderer ren;
    private string id;
    private long timeCounter;

    public Material alarmMat;
    public Material normalMat; 

    void Awake()
    {
        alarm = false;
        ren = GetComponent<Renderer>();
        id = "SmokeSensor";
    }

    void Start () { } 
	void Update ()
    {
        if (alarm)
        {
            long deltaTime = DateTime.Now.Second - timeCounter;
            if(deltaTime == 5)
            {
                alarm = false;
                ren.material = normalMat;
                GameObject.Find("CentralSystem").SendMessage("UnSetAlarm", id);
                transform.parent.GetComponentInChildren<SmartPanel>().SwitchAlarm();
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        timeCounter = System.DateTime.Now.Second;
        if (!alarm)
        {
            alarm = true;
            ren.material = alarmMat;
            GameObject.Find("CentralSystem").SendMessage("SetAlarm",id);
            transform.parent.GetComponentInChildren<SmartPanel>().SwitchAlarm();
        }
    }

    public string GetState()
    {
        return alarm ? "alarm" : "noAlarm" ;
    }
}
