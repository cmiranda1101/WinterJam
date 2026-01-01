using System.Threading.Tasks;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using static Unity.Behavior.Node;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public NavMeshAgent navMeshAgent;
    [SerializeField] BehaviorGraphAgent behaviorGraphAgent;

    //Variables for movement
    [SerializeField] public float speed;
    [SerializeField] private float angleThreshold;
    [SerializeField] private float rotSpeed;
    private Quaternion targetRotation;

    BlackboardVariable<bool> isMoving;

    void Start()
    {
        navMeshAgent.speed = speed;
    }

    public Status PatrolArea()
    {
        //If not currently moving, pick a random point within a radius and set it as the destination
        behaviorGraphAgent.GetVariable("isMoving", out isMoving);
        if (!isMoving)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                behaviorGraphAgent.SetVariableValue("isMoving", true);
            }
        }
        else
        {
            //Check if the agent has reached its destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                behaviorGraphAgent.SetVariableValue("isMoving", false);
                return Status.Success;
            }
        }
        return Status.Running;
    }

    public void SetDestination(Vector3 destination)
    {
        //Sample the NavMesh to find a valid position close to the destination
        NavMeshHit hit;
        if(NavMesh.SamplePosition(destination, out hit, 10f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
        }
    }

    public Status RotateTowardsPlayer(Vector3 playerPosition)
    {
        //Normalize direction and ignore y-axis for rotating on the horizontal plane
        Vector3 direction = (playerPosition - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        //Slerp towards the target rotation to avoid snapping
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

        //Check if the rotation is within the angle threshold to the target rotation
        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        //If within threshold, return Success, else Running
        return angle <= angleThreshold ? Status.Success : Status.Running;
    }
}
