using UnityEngine;
using UnityEngine.AI;

public class PersonCreator : MonoBehaviour
{

    public GameObject person;
    public int people_numbers = 100;

    void Awake()
    {
        int i = 0;
        for (i = 0; i < people_numbers; i++)
        {
            PersonSpawn(i);
        }
    }

    public GameObject PersonSpawn(int id)
    {
        for (int i = 0; i < 100; i++)
        {
            bool found = false;
            Vector3 point;
            RandomPoint(Vector3.zero, 50, out point);
            Collider[] colliders = Physics.OverlapSphere(point, 1f);
            foreach (Collider collider in colliders)
            {
                Person pb = collider.gameObject.GetComponent<Person>();
                if (pb != null)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                GameObject people = GameObject.Find("People");
                if (!people) people = new GameObject("People");
                GameObject person_obj = Instantiate(person);
                person_obj.transform.SetParent(people.transform);
                person_obj.transform.position = new Vector3(point.x, person_obj.transform.localScale.y, point.z);
                return person_obj;
            }
        }


        return null;
    }


    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
