using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetCover", story: "[Agent] Finds Cover", category: "Action", id: "7d8b01d4633862f629200e92a85113d8")]
public partial class GetCoverAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    EnemyController enemyController;
    protected override Status OnStart()
    {
        enemyController = Agent.Value.GetComponent<EnemyController>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (enemyController == null)
        {
            Debug.LogError("EnemyController component not found on Agent.");
            return Status.Failure;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

