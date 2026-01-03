using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    public int numberOfRoomsToGenerate; //The number of rooms that will be generated in the level. Does not include the starting room

    GameObject player;
    RoomManager nextRoomManager;
    int currentGridPosX = 0;
    int currentGridPosY = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateStartingRoom();
        player = GameObject.FindWithTag("Player");
    }

    void GetRoomManagerScript(GameObject _roomInstance)
    {
        nextRoomManager = _roomInstance.transform.GetChild(0).GetComponent<RoomManager>(); //room manager should always be the first child of any room
    }

    void GenerateStartingRoom()
    {
        GameObject roomToSpawn = rooms[Random.Range(0, rooms.Length)];
        GameObject startingRoomInstance = Instantiate(roomToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
        GetRoomManagerScript(startingRoomInstance);
        nextRoomManager.gridPosX = 0;
        nextRoomManager.gridPosY = 0;
        GameObject childWithTag;
        for (int i = 0; i < roomToSpawn.transform.childCount; i++)
        {
            childWithTag = startingRoomInstance.transform.GetChild(i).gameObject;
            if (childWithTag.tag == "Door")
            {
                childWithTag.SetActive(true);
            }
        }
    }

    public void GenerateRoom(Vector3 _roomSpawnPos, DoorDirection _directionEnteredFrom)
    {
        GameObject roomToSpawn = rooms[Random.Range(0, rooms.Length)];
        GameObject roomInstance = Instantiate(roomToSpawn, _roomSpawnPos, Quaternion.identity);
        GetRoomManagerScript(roomInstance);
        nextRoomManager.UpdateGridPos(currentGridPosX, currentGridPosY, _directionEnteredFrom);
        currentGridPosX = nextRoomManager.gridPosX;
        currentGridPosY = nextRoomManager.gridPosY;
        GameObject childWithTag;
        List<GameObject> doors = new List<GameObject>();
        for (int i = 0; i < roomInstance.transform.childCount; i++)
        {
            childWithTag = roomInstance.transform.GetChild(i).gameObject;
            if (childWithTag.tag == "Door")
            {
                doors.Add(childWithTag);
                childWithTag.SetActive(false);
            }
        }
        int numberOfDoorsToSpawn = Random.Range(2, doors.Count);
        for (int i = 0; i < doors.Count; i++)
        {
            nextRoomManager.CheckIfDoorIsInvalid(roomInstance, doors[i].transform.GetChild(1).gameObject);
        }
        SpawnEssentialDoors(doors);
        SpawnRandomDoor(doors, numberOfDoorsToSpawn);
        //HandleInvalidDoors(doors);
        numberOfRoomsToGenerate--;
    }

    void SpawnEssentialDoors(List<GameObject> _doorsNotSpawned)
    {
        for (int i = 0; i < _doorsNotSpawned.Count; i++)
        {
            if (_doorsNotSpawned[i].transform.GetChild(1).GetComponent<Door>().isDoorEssential)
            {
                _doorsNotSpawned[i].SetActive(true);
            }
        }
    }

    void HandleInvalidDoors(List<GameObject> _doors)
    {
        for (int i = 0; i < _doors.Count; i++)
        {
            if (_doors[i].transform.GetChild(1).GetComponent<Door>().doesNextRoomExist && _doors[i].transform.GetChild(1).GetComponent<Door>().isDoorInvalid)
            {
                _doors[i].SetActive(false);
            }
        }
    }

    void SpawnRandomDoor(List<GameObject> _doorsNotSpawned, int _numberOfDoorsToSpawn)
    {
        int doorToSpawn = Random.Range(0, _doorsNotSpawned.Count);
        if (!_doorsNotSpawned[doorToSpawn].transform.GetChild(1).GetComponent<Door>().isDoorInvalid) //Checks if door leads to a room that does not have a corresponding door
        {
            _doorsNotSpawned[doorToSpawn].SetActive(true);
        }
        _doorsNotSpawned.Remove(_doorsNotSpawned[doorToSpawn]);
        _numberOfDoorsToSpawn--;
        if (_numberOfDoorsToSpawn > 0)
        {
            SpawnRandomDoor(_doorsNotSpawned, _numberOfDoorsToSpawn);
        }
    }
}
