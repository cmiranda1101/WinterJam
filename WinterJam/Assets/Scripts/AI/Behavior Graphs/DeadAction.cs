using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Dead", story: "[Agent] [EnemyAI] [isDead]", category: "Action", id: "8a4ce6b740609b8b1ef8d69ccbf6d48f")]
public partial class DeadAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<EnemyAI> EnemyAI;
    [SerializeReference] public BlackboardVariable<bool> isDead;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(isDead == true)
        {
            return Status.Running;
        }
        return Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

