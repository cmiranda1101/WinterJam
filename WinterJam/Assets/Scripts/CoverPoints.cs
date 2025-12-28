using System.Collections.Generic;
using UnityEngine;

// Put this script on cover game objects with a trigger collider to track cover points on a piece of cover for enemies.
public class CoverPoints : MonoBehaviour
{
    [SerializeField] public List<Transform> coverPoints;
    private AIState currentState;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null && enemyAI.behaviorGraphAgent.BlackboardReference.GetVariableValue("AIState", out currentState))
            {
                Transform closestTransform = coverPoints[0].transform;
                for (int i = 0; i < coverPoints.Count; i++)
                {
                    float distanceToCoverPoint = Vector3.Distance(other.transform.position, coverPoints[i].position);
                    float distanceToCurrentBest = Vector3.Distance(other.transform.position, closestTransform.position);
                    if (distanceToCoverPoint < distanceToCurrentBest)
                    {
                        closestTransform = coverPoints[i].transform;
                    }
                }
                enemyAI.enemyController.SetDestination(closestTransform.transform.position);
            }
        }
    }
}
