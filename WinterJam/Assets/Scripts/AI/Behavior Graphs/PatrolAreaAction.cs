using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolArea", story: "[Agent] patrols in a random area", category: "Action", id: "22fac30b3fa5689a60d79cec6643c797")]
public partial class PatrolAreaAction : Action
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
        return enemyController.PatrolArea();
    }

    protected override void OnEnd()
    {
    }
}

