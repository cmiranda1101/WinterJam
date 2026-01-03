using UnityEngine;
using System.Collections.Generic;

public enum DoorDirection
{
    North,
    South,
    West,
    East
};

public class Door : MonoBehaviour
{
    public DoorDirection direction;

    GameObject levelGenerator;
    LevelGenerator levelGeneratorScript;
    GameObject room;
    GameObject roomManager;
    RoomManager roomManagerScript;
    GameObject player;
    CharacterController playerCharacterController;


    public bool isDoorInvalid = false;
    public bool isDoorEssential = false;
    public bool doesNextRoomExist = false;
    public GameObject neighboringRoom;
    int enemiesAlive;
    Vector3 newRoomSpawnPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
        levelGeneratorScript = levelGenerator.GetComponent<LevelGenerator>();
        room = transform.parent.transform.parent.gameObject;
        roomManager = room.transform.GetChild(0).gameObject; //Make sure the room manager is always the first child of the room
        roomManagerScript = roomManager.GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCharacterController = player.GetComponent<CharacterController>();
    }

    void GoToNextRoom()
    {
        if (!doesNextRoomExist)
        {
            if (levelGeneratorScript.numberOfRoomsToGenerate > 0)
            {
                newRoomSpawnPos = transform.position - (gameObject.transform.forward * 30);
                newRoomSpawnPos.y = 0;
                levelGeneratorScript.GenerateRoom(newRoomSpawnPos, direction);
                doesNextRoomExist = true;
                TeleportPlayerToRoom();
            }
        }
        else
        {
            TeleportPlayerToRoom();
        }
    }

    void TeleportPlayerToRoom()
    {
        playerCharacterController.enabled = false;
        Vector3 newPlayerPos = transform.position - (transform.forward * 10);
        newPlayerPos.y = 1;
        player.transform.position = newPlayerPos;
        playerCharacterController.enabled = true;
    }

    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GoToNextRoom();
        }
    }
}
