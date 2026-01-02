using UnityEngine;
using Unity.Behavior;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] public EnemyController enemyController;
    [SerializeField] public EnemyAnimator enemyAnimator;
    [SerializeField] public EnemyWeapon enemyWeapon;
    [HideInInspector] public CoverObjects coverObjects;

    GameObject player;

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
}
