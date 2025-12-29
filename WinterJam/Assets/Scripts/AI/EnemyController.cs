using UnityEngine;
using UnityEngine.AI;
using static Unity.Behavior.Node;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    
    public bool isMoving = false;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public Status PatrolArea()
    {
        if (!isMoving)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                isMoving = true;
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isMoving = false;
                return Status.Success;
            }
        }
        return Status.Running;
    }

    public void SetDestination(Vector3 destination)
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(destination, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            isMoving = true;
        }
    }
}
