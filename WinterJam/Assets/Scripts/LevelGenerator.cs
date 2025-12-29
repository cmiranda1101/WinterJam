using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    public int numberOfRoomsToGenerate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateRoom(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRoom(Vector3 _roomSpawnPos)
    {
        GameObject roomToSpawn = rooms[Random.Range(0, rooms.Length)];
        Instantiate(roomToSpawn, _roomSpawnPos, Quaternion.identity);
        numberOfRoomsToGenerate--;   
    }
}
