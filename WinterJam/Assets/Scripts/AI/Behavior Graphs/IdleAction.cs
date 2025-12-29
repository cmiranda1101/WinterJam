using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Idle", story: "Waits until [isInitialized]", category: "Action", id: "1afbda3bc34c6f48cfbfeaff66c712dd")]
public partial class IdleAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> isInitialized;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(isInitialized.Value)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

