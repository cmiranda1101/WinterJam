using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GetCover", story: "[Agent] Finds [Cover] Sets [EnemyAI] State And [CurrentCover] And Faces [Player] Checks If [isMoving]", category: "Action", id: "7d8b01d4633862f629200e92a85113d8")]
public partial class GetCoverAction : Action
{
    [SerializeReference] public BlackboardVariable<EnemyAI> Enemy;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<PlayerController> Player;
    [SerializeReference] public BlackboardVariable<CoverObjects> Cover;
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
        if (Enemy.Value.enemyController == null)
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
        
        //Check if the agent has reached its destination
        if(!Enemy.Value.enemyController.navMeshAgent.pathPending && Enemy.Value.enemyController.navMeshAgent.remainingDistance <= Enemy.Value.enemyController.navMeshAgent.stoppingDistance)
        {
            isMoving.Value = false;
        }
        //If the agent is still moving towards cover, return running
        if (isMoving.Value)
        {
            return Status.Running;
        }
        //If the agent does not have a current cover, find the closest available cover
        else if (CurrentCover.Value == null)
        {
            //Find the closest available cover
            foreach (var cover in Cover.Value.coverObjects)
            {
                //Skip occupied or reserved cover
                if (cover.Value != CoverState.Empty) { continue; }

                distanceToCover = cover.Key.transform.position - Agent.Value.transform.position;
                if (closestCoverPosition == Vector3.zero ||
                    distanceToCover.magnitude < (closestCoverPosition - Agent.Value.transform.position).magnitude)
                {
                    closestCoverPosition = cover.Key.transform.position;
                    CurrentCover = (BlackboardVariable<GameObject>)cover.Key;
                }
            }
            
            //If found available cover, move to it and reserve it
            if(CurrentCover.Value != null)
            {
                Enemy.Value.enemyAnimator.SetAnimationBool("isStrafe", false);
                Enemy.Value.enemyController.SetDestination(closestCoverPosition);
                isMoving.Value = true;
                Cover.Value.coverObjects[CurrentCover.Value] = CoverState.Reserved;
                return Status.Running;
            }
            //If no available cover, switch to attack state
            else if(CurrentCover.Value == null)
            {
                Enemy.Value.behaviorGraphAgent.SetVariableValue("AIState", AIState.Attack);
                return Status.Success;
            }
        }
        //If the agent has reached the cover, set state to attack, face the player, and mark cover as occupied
        else if (CurrentCover.Value != null && !isMoving.Value)
        {
            Enemy.Value.behaviorGraphAgent.SetVariableValue("inCover", true);
            Enemy.Value.behaviorGraphAgent.SetVariableValue("AIState", AIState.Attack);
            Cover.Value.coverObjects[CurrentCover.Value] = CoverState.Occupied;
            return Enemy.Value.enemyController.RotateTowardsPlayer(Player.Value.transform.position);
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

