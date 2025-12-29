using UnityEngine;

public class Door : MonoBehaviour
{
    GameObject levelGenerator;
    LevelGenerator levelGeneratorScript;
    GameObject player;
    CharacterController playerCharacterController;

    bool roomGenerated = false;
    public int enemiesAlive;
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
        if (roomGenerated == false)
        {
            Vector3 newRoomSpawnPos = transform.position - (transform.forward * 30);
            newRoomSpawnPos.y = 0;
            levelGeneratorScript.GenerateRoom(newRoomSpawnPos);
            roomGenerated = true;
        }
        playerCharacterController.enabled = false;
        Vector3 newPlayerPos = transform.position - (transform.forward * 10);
        newPlayerPos.y = 1;
        player.transform.position =newPlayerPos;
        playerCharacterController.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            CheckEnemiesAlive();
            if (enemiesAlive <= 0)
            {
                GoToNextRoom();
            }
        }
    }
}
