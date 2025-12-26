using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using Unity.Behavior;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] BehaviorGraphAgent behaviorGraphAgent;

    [SerializeField] EnemyController enemyController;

    NavMeshAgent agent;
    NavMeshController navMeshController;

    void Awake()
    {
        behaviorGraphAgent.BlackboardReference.SetVariableValue("self", this);

        behaviorGraphAgent.Init();
    }

    void Start()
    {
        navMeshController = GetComponentInParent<NavMeshController>();
        _ = EnableAgentWhenNavMeshReady(navMeshController);
    }

    void Update()
    {
        
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
}
