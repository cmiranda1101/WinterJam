using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// This Script manages AI enemies within a room by tracking them in the list and updating their behavior when the player enters the room
public class AIRoomManager : MonoBehaviour
{
    [SerializeField] BoxCollider roomCollider;

    List<GameObject> enemiesInRoom;
    void Start()
    {
        enemiesInRoom = new List<GameObject>();
        
        roomCollider.enabled = true;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy"))
            {
                enemiesInRoom.Add(child.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemiesInRoom)
            {
                EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.behaviorGraphAgent.BlackboardReference.SetVariableValue("AIState", AIState.GetCover);
                    enemyAI.behaviorGraphAgent.BlackboardReference.SetVariableValue("playerInRoom", true);
                }
            }
        }
    }
}
