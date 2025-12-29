using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using Unity.Behavior;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] public EnemyController enemyController;
    [HideInInspector] public CoverObjects coverObjects;

    NavMeshAgent agent;

    GameObject player;

    void Awake()
    {
        behaviorGraphAgent.Init();
    }

    void Start()
    {
        coverObjects = GetComponentInParent<CoverObjects>();
        EnableAgent();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
    }

    public void EnableAgent()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.enabled = true;
        behaviorGraphAgent.BlackboardReference.SetVariableValue("self", gameObject);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("EnemyAI", this);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("EnemyController", enemyController);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("CoverObjects", coverObjects);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("isInitialized", true);
    }
}
