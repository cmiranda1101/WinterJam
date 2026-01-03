using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public NavMeshAgent navMeshAgent;
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void SetAnimationBool(string parameterName, bool value)
    {
        animator.SetBool(parameterName, value);
    }
    
    public void SetAnimationSpeed()
    {
        float speedNormalized = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetFloat("Speed", speedNormalized, 0.1f, Time.deltaTime);
    }

    public void SetAnimationStrafeDirection(float strafeDirectionNormalized)
    {
        animator.SetFloat("StrafeDir", strafeDirectionNormalized, 0.1f, Time.deltaTime);
    }
}
