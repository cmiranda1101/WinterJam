using UnityEngine;

public class EnemyRegister : MonoBehaviour
{
    [HideInInspector] public AIRoomManager roomManager;
    
    void Start()
    {
        roomManager = GetComponentInParent<AIRoomManager>();
        RegisterToRoom();
    }

    public void RegisterToRoom()
    {
        roomManager.enemiesInRoom.Add(gameObject);
    }
}
