using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolArea", story: "[Agent]/[EnemyAI] patrols in a random area", category: "Action", id: "22fac30b3fa5689a60d79cec6643c797")]
public partial class PatrolAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<EnemyAI> Enemy;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Enemy.Value == null)
        {
            Debug.LogWarning("PatrolAreaAction.cs: EnemyAI component not found on Agent.");
            return Status.Running;
        }
        else
        {
            Enemy.Value.enemyAnimator.SetAnimationBool("isStrafe", false);
            return Enemy.Value.enemyController.PatrolArea();
        }
    }

    protected override void OnEnd()
    {
    }
}

