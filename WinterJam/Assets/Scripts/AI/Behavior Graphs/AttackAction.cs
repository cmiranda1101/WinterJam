using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack", story: "[Agent] Attacks [Player] if [inCover] Or Freeroaming Moves With [EnemyController] Sets [Animator]", category: "Action", id: "a7d91e9cbf2c124024f9f5a96a3c45e2")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<EnemyController> EnemyController;
    [SerializeReference] public BlackboardVariable<EnemyAnimator> Animator;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<bool> inCover;
    private float timer = 0f;
    private float strafeTimer = 0f;
    private float pauseTimer = 0f;
    private float targetAngle = 0f;
    private float radius = 0f;
    protected override Status OnStart()
    {
        pauseTimer = UnityEngine.Random.Range(0f, 3f);
        strafeTimer = UnityEngine.Random.Range(0f, 4f);
        targetAngle = UnityEngine.Random.Range(-90f, 90f);
        radius = UnityEngine.Random.Range(15f, 30f);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        timer += Time.deltaTime;
        //If in cover, attack from cover (not implemented yet)
        if (inCover == true)
        {

        }
        //If not in cover, move towards player if too far, or else strafe around them
        else
        {
            Vector3 distanceToPlayer = Agent.Value.transform.position - Player.Value.transform.position;
            float distanceMagnitude = distanceToPlayer.magnitude;
            //if too far away, move closer (distance is 100 since distance is not sqrt), else strafe around player
            if (distanceMagnitude > 100f)
            {
                Animator.Value.SetAnimationBool("isStrafe", false);
                EnemyController.Value.SetDestination(Player.Value.transform.position);
            }
            //If within range, strafe
            else
            {
                //If times is less than strafe time, strafe
                if (timer < strafeTimer)
                {
                    Animator.Value.SetAnimationBool("isStrafe", true);
                    //Gets normalized value between for 0-1 animation value
                    float normalizedDir = Mathf.InverseLerp(90f, -90f, targetAngle);
                    Animator.Value.SetAnimationStrafeDirection(normalizedDir);
                    //strafe around player in a half-circle
                    Vector3 directionToPlayer = distanceToPlayer.normalized;
                    //rotates the normalized direction vector 90 degrees around the player
                    Vector3 rotatedDirection = Quaternion.Euler(0, targetAngle, 0) * directionToPlayer;
                    Vector3 strafePosition = Player.Value.transform.position + rotatedDirection * radius; //Range 20-40 units away from player (radius)
                    EnemyController.Value.SetDestination(strafePosition);
                    _ = EnemyController.Value.RotateTowardsPlayer(Player.Value.transform.position);
                }
                //If timer is greater than strafe time but less than pause time, stop moving and face player
                else if (timer > strafeTimer && timer < strafeTimer + pauseTimer)
                {
                    Animator.Value.SetAnimationBool("isStrafe", false);
                    EnemyController.Value.SetDestination(Agent.Value.transform.position); //stop moving
                    _ = EnemyController.Value.RotateTowardsPlayer(Player.Value.transform.position);
                }
                //If timer is greater than strafe time and pause time, reset timer and change the strafe direction
                else
                {
                    timer = 0f;
                    targetAngle *= -1; //switch strafe direction
                    //Gets normalized value between for 0-1 animation value
                    float normalizedDir = Mathf.InverseLerp(-90f, 90f, targetAngle);
                    Animator.Value.SetAnimationStrafeDirection(normalizedDir);
                }
            }
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

