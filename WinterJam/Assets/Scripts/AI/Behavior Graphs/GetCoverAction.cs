using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetCover", story: "[Agent] Finds [Cover] Using [Controller] Sets [EnemyAI] State And [CurrentCover] And Faces [Player] Checks If [isMoving]", category: "Action", id: "7d8b01d4633862f629200e92a85113d8")]
public partial class GetCoverAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<PlayerController> Player;
    [SerializeReference] public BlackboardVariable<CoverObjects> Cover;
    [SerializeReference] public BlackboardVariable<EnemyController> Controller;
    [SerializeReference] public BlackboardVariable<EnemyAI> Enemy;
    [SerializeReference] public BlackboardVariable<bool> isMoving;
    [SerializeReference] public BlackboardVariable<GameObject> CurrentCover;
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
        if(!Controller.Value.navMeshAgent.pathPending && Controller.Value.navMeshAgent.remainingDistance <= Controller.Value.navMeshAgent.stoppingDistance)
        {
            isMoving.Value = false;
        }
        if(isMoving.Value)
        {
            return Status.Running;
        }
        else if (CurrentCover.Value == null)
        {
            foreach (var cover in Cover.Value.coverObjects)
            {
                if(cover.Value != CoverState.Empty) { continue; }

                distanceToCover = cover.Key.transform.position - Agent.Value.transform.position;
                if (closestCoverPosition == Vector3.zero ||
                    distanceToCover.magnitude < (closestCoverPosition - Agent.Value.transform.position).magnitude)
                {
                    closestCoverPosition = cover.Key.transform.position;
                    CurrentCover = (BlackboardVariable<GameObject>)cover.Key;
                }
            }
            
            if(CurrentCover.Value != null)
            {
                Controller.Value.SetDestination(closestCoverPosition);
                isMoving.Value = true;
                Cover.Value.coverObjects[CurrentCover.Value] = CoverState.Reserved;
                return Status.Running;
            }
            else if(CurrentCover.Value == null)
            {
                Enemy.Value.behaviorGraphAgent.SetVariableValue("AIState", AIState.Attack);
                return Status.Success;
            }
        }
        else if (CurrentCover.Value != null && !isMoving.Value)
        {
            Enemy.Value.behaviorGraphAgent.SetVariableValue("inCover", true);
            Enemy.Value.behaviorGraphAgent.SetVariableValue("AIState", AIState.Attack);
            Cover.Value.coverObjects[CurrentCover.Value] = CoverState.Occupied;
            return Controller.Value.RotateTowardsPlayer(Player.Value.transform.position);
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

