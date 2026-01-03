using UnityEngine;
using Unity.Behavior;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] public BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] public EnemyController enemyController;
    [SerializeField] public EnemyAnimator enemyAnimator;
    [SerializeField] public EnemyWeapon enemyWeapon;
    [SerializeField] public IceEffect enemyEffect;
    [HideInInspector] public CoverObjects coverObjects;

    GameObject player;

    [SerializeField] public float health;
    void Awake()
    {
        behaviorGraphAgent.Init();
    }

    void Start()
    {
        coverObjects = GetComponentInParent<CoverObjects>();
        player = GameObject.FindGameObjectWithTag("Player");
        InitializeBehaviorGraph();
    }

    void Update()
    {
        enemyAnimator.SetAnimationSpeed();
    }

    //Initialize blackboard variables for the behavior graph
    public void InitializeBehaviorGraph()
    {
        behaviorGraphAgent.BlackboardReference.SetVariableValue("self", gameObject);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("Player", player);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("EnemyAI", this);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("CoverObjects", coverObjects);
        behaviorGraphAgent.BlackboardReference.SetVariableValue("isInitialized", true);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            enemyController.navMeshAgent.SetDestination(transform.position);
            enemyAnimator.SetAnimationBool("isDead", true);
            behaviorGraphAgent.SetVariableValue("AIState", AIState.Dead);
            enemyEffect.SpawnIceBlock();
        }
    }
}
