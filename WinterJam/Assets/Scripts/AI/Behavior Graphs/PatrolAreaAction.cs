using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolArea", story: "[Agent] patrols in a random area using [Controller]", category: "Action", id: "22fac30b3fa5689a60d79cec6643c797")]
public partial class PatrolAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Controller.Value == null)
        {
            Debug.LogWarning("PatrolAreaAction.cs: EnemyController component not found on Agent.");
            return Status.Running;
        }
        else
        {
            return Controller.Value.PatrolArea();
        }
    }

    protected override void OnEnd()
    {
    }
}

