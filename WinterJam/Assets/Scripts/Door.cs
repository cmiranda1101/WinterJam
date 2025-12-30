using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject levelGenerator;
    LevelGenerator levelGeneratorScript;
    GameObject player;
    CharacterController playerCharacterController;

    //bool roomGenerated = false;
    int enemiesAlive;
    Vector3 newRoomSpawnPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
        levelGeneratorScript = levelGenerator.GetComponent<LevelGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCharacterController = player.GetComponent<CharacterController>();
        CheckEnemiesAlive();
    }

    void CheckEnemiesAlive()
    {
        enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void GoToNextRoom()
    {
        newRoomSpawnPos = transform.position - (transform.forward * 30);
        if (!CheckIfRoomExists(newRoomSpawnPos))
        {
            if (levelGeneratorScript.numberOfRoomsToGenerate > 0)
            {
                newRoomSpawnPos.y = 0;
                levelGeneratorScript.GenerateRoom(newRoomSpawnPos);
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

    bool CheckIfRoomExists(Vector3 _posToCheck)
    {
        bool doesRoomExist = false;
        _posToCheck.y = 20;
        RaycastHit hit;
        if (Physics.Raycast(_posToCheck, Vector3.down, out hit))
        {
            if (hit.collider != null)
            {
                doesRoomExist = true;
            }
        }
        return doesRoomExist;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckEnemiesAlive();
            if (enemiesAlive <= 0)
            {
                GoToNextRoom();
            }
            GoToNextRoom();
        }
    }
}
