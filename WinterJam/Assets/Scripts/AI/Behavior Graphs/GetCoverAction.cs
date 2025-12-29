using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetCover", story: "[Agent] Finds [Cover] Using [Controller] Sets [EnemyAI] State", category: "Action", id: "7d8b01d4633862f629200e92a85113d8")]
public partial class GetCoverAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<CoverObjects> Cover;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    [SerializeReference] public BlackboardVariable<EnemyAI> Enemy;
    Vector3 distanceToCover;
    Vector3 closestCoverPosition;
    protected override Status OnStart()
    {
        distanceToCover = new Vector3(0, 0, 0);
        closestCoverPosition = new Vector3(0, 0, 0);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Controller.Value == null)
        {
            Debug.LogWarning("GetCoverAction.cs: EnemyController component not found on Agent.");
            return Status.Failure;
        }
        if (Cover.Value == null)
        {
            Debug.LogWarning("GetCoverAction.cs: CoverObjects component not found on Agent.");
            return Status.Failure;
        }
        if (Enemy.Value == null)
        {
            Debug.LogWarning("GetCoverAction.cs: EnemyAI component not found on Agent.");
            return Status.Failure;
        }
        if(Controller.Value.isMoving)
        {
            return Status.Running;
        }
        else
        {
            foreach (var cover in Cover.Value.coverObjects)
            {
                distanceToCover = cover.transform.position - Agent.Value.transform.position;
                if (closestCoverPosition == Vector3.zero ||
                    distanceToCover.magnitude < (closestCoverPosition - Agent.Value.transform.position).magnitude)
                {
                    closestCoverPosition = cover.transform.position;
                }
            }
            Controller.Value.SetDestination(closestCoverPosition);
            Enemy.Value.behaviorGraphAgent.SetVariableValue("AIState", AIState.Attack);
            return Status.Success;
        }
    }

    protected override void OnEnd()
    {
    }
}

