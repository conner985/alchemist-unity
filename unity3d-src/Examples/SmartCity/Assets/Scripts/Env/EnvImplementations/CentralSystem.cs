using UnityEngine;

public class CentralSystem : MonoBehaviour,ICentralSystem {

    private int alarmedDevices;

    void Start()
    {
        Debug.Log("people: " + FindObjectsOfType<Person>().Length);
    }
     
    public void SetAlarm(string id)
    {
        alarmedDevices++;
        if (alarmedDevices == 1)
        {
            AbstractBehaviourNode[] g_scripts = FindObjectsOfType<AbstractBehaviourNode>();
            foreach (AbstractBehaviourNode g in g_scripts)
            {
                g.gameObject.SendMessage("ReceiveInfo", "SET");
            }

            Person[] pbs = FindObjectsOfType<Person>();
            foreach (Person pb in pbs)
            {
                pb.gameObject.SendMessage("ReceiveInfo", "SET");
            }
        }
    }

    public void UnSetAlarm(string id)
    {
        alarmedDevices--;
        if (alarmedDevices == 0)
        {
            AbstractBehaviourNode[] g_scripts = FindObjectsOfType<AbstractBehaviourNode>();
            foreach (AbstractBehaviourNode g in g_scripts)
            {
                g.gameObject.SendMessage("ReceiveInfo", "UNSET");
            }

            Person[] pbs = FindObjectsOfType<Person>();
            foreach (Person pb in pbs)
            {
                pb.gameObject.SendMessage("ReceiveInfo", "UNSET");
            }
        }
    }
}
