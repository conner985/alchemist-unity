using UnityEngine;
using UnityEngine.AI;
using System;

public class Person : MonoBehaviour
{

    enum state
    {
        IDLE,
        CHOOSEPOI,
        EXPLORE,
        PANIC,
        DEAD,
    }

    [SerializeField]
    private float walkSpeed = 2f;
    [SerializeField]
    private float runSpeed = 5f;
    [SerializeField]
    private float commRadius = 6f;

    private NavMeshAgent agent;
    private System.Random rnd;

    private state curState;
    private float idleTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        curState = state.CHOOSEPOI;
        idleTime = float.NaN;
        rnd = new System.Random(DateTime.Now.Millisecond);
    }

    void Update()
    {

        if (rnd.NextDouble() > 0.4f) return;
        FSM();
    }

    public void ReceiveInfo(object info)
    {
        switch (info.ToString())
        {
            case "SET":
                curState = state.PANIC;
                break;
            case "UNSET":
                curState = state.CHOOSEPOI;
                break;
        }
    }

    private void FSM()
    {
        switch (curState)
        {
            case (state.IDLE):
                Idle();
                break;
            case (state.CHOOSEPOI):
                ChoosePOI();
                break;
            case (state.EXPLORE):
                agent.speed = walkSpeed;
                Explore();
                break;
            case (state.PANIC):
                agent.speed = runSpeed;
                Panic();
                break;
            case (state.DEAD):
                Dead();
                break;
        }
    }

    private void Dead()
    {
        //TODO: la gente che passa sul fuoco muore
    }

    private void Panic()
    {
        if (rnd.NextDouble() < 0.8f) return;

        double gradient = double.MaxValue - 1;
        Collider[] colliders = Physics.OverlapSphere(transform.position, commRadius);
        foreach (Collider collider in colliders)
        {
            AbstractBehaviourNode behaviourNode = collider.gameObject.GetComponent<AbstractBehaviourNode>();
            if (behaviourNode != null)
            {
                ISimNode simNode = behaviourNode.GetNode();
                if (simNode.GetType().Equals(typeof(GradientNode)))
                {
                    GradientNode gradientNode = simNode as GradientNode;
                    if (Convert.ToDouble(gradientNode.GetMolecules().GetMoleculeConcentration("data")) < gradient)
                    {
                        gradient = Convert.ToDouble(gradientNode.GetMolecules().GetMoleculeConcentration("data"));
                        Vector3 destination = collider.transform.position;
                        agent.SetDestination(new Vector3(destination.x, transform.position.y, destination.z));
                    }
                }
            }
        }
    }

    private void Explore()
    {
        if (agent.remainingDistance < 1) curState = state.IDLE;
    }

    private void ChoosePOI()
    {
        Vector3 point;
        RandomPoint(Vector3.zero, 50, out point);
        Vector3 destination = point;
        agent.SetDestination(new Vector3(destination.x, transform.position.y, destination.z));
        curState = state.EXPLORE;
    }

    private void Idle()
    {
        if (float.IsNaN(idleTime))
        {
            idleTime = new System.Random(System.DateTime.Now.Millisecond).Next(2, 8);
        }
        else if (idleTime > 0)
        {
            idleTime -= Time.deltaTime;
        }
        else
        {
            idleTime = float.NaN;
            curState = state.CHOOSEPOI;
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
