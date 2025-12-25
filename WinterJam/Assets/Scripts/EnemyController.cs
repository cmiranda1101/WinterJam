using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    NavMeshController navMeshController;

    bool isMoving = false;

    void Start()
    {
        navMeshController = GetComponentInParent<NavMeshController>();
        _ = EnableAgentWhenNavMeshReady(navMeshController);
    }

    void Update()
    {
        if(agent != null)
        PatrolArea();
    }

    public async Task EnableAgentWhenNavMeshReady(NavMeshController navMeshController)
    {
        while (navMeshController.isBaking)
        {
            await Task.Yield();
        }
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.enabled = true;
    }

    public void PatrolArea()
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
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(destination, out hit, 1f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
