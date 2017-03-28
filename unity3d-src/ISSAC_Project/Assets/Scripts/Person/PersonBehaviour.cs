using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody)),RequireComponent(typeof(NavMeshAgent))]
public class PersonBehaviour : MonoBehaviour {

    enum state
    {
        CHOOSEPOI,
        EXPLORE
    }

    [SerializeField]
    private float range = 10.0f;

    private Node node;

    private NavMeshAgent agent;

    private state curState; 

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        NodesCollector nc = FindObjectOfType<NodesCollector>();
        node = new Node(nc.GenerateNewID(), 0f, new NodePosition2D(transform.position.x, transform.position.z));

        curState = state.CHOOSEPOI;
    }

    void Update ()
    {
        FSM();
    }

    private void FSM()
    {
        switch (curState)
        {
            case (state.CHOOSEPOI):
                ChoosePOI();
                break;
            case (state.EXPLORE):
                Explore();
                break;
        }
    }

   
    private void Explore()
    {
        if (agent.remainingDistance < 0.1) curState = state.CHOOSEPOI;
    }

    private void ChoosePOI()
    {
		Vector3 point;
        RandomPoint(transform.position, range, out point);
        agent.SetDestination(new Vector3(point.x, transform.position.y, point.z));
        curState = state.EXPLORE;
    }

	private bool RandomPoint(Vector3 center, float range, out Vector3 result) {
		for (int i = 0; i < 30; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}

    public Node GetNode()
    {
        return node;
    }
}
