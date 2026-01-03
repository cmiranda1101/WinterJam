using UnityEngine;

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
    GameObject player;
    CharacterController playerCharacterController;


    public bool isDoorInvalid = false;
    public bool isDoorEssential = false;
    public GameObject neighboringRoom;
    Vector3 newRoomSpawnPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
        levelGeneratorScript = levelGenerator.GetComponent<LevelGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCharacterController = player.GetComponent<CharacterController>();
    }

    void GoToNextRoom()
    {
        if (neighboringRoom == null)
        {
            if (levelGeneratorScript.numberOfRoomsToGenerate > 0)
            {
                newRoomSpawnPos = transform.position - (gameObject.transform.forward * 30);
                newRoomSpawnPos.y = 0;
                neighboringRoom = levelGeneratorScript.GenerateRoom(newRoomSpawnPos, direction);
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
        float playerY = player.transform.position.y; // makes sure the player isn't floating or sinks into the ground
        Vector3 newPlayerPos = transform.position - (transform.forward * 10);
        newPlayerPos.y = playerY;
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
