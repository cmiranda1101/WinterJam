using System.Collections.Generic;
using UnityEngine;

// Put this script on cover game objects with a trigger collider to track cover points on a piece of cover for enemies.
public class CoverPoints : MonoBehaviour
{
    [SerializeField] public List<Transform> coverPoints;
    GameObject player;
    async private void Start()
    {
        await System.Threading.Tasks.Task.Delay(100); // Small delay to ensure other systems are initialized
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                Transform closestTransform = coverPoints[0].transform;
                for (int i = 0; i < coverPoints.Count; i++)
                {
                    float distanceToPlayer = Vector3.Distance(player.transform.position, coverPoints[i].position);
                    float furthestFromPlayer = Vector3.Distance(player.transform.position, closestTransform.position);
                    if (distanceToPlayer > furthestFromPlayer)
                    {
                        closestTransform = coverPoints[i].transform;
                    }
                }
                enemyAI.enemyController.SetDestination(closestTransform.transform.position);
            }
        }
    }
}
