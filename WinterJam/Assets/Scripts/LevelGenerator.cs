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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateStartingRoom();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateStartingRoom()
    {
        GameObject roomToSpawn = rooms[Random.Range(0, rooms.Length)];
        GameObject childWithTag;
        for (int i = 0; i < roomToSpawn.transform.childCount; i++)
        {
            childWithTag = roomToSpawn.transform.GetChild(i).gameObject;
            if (childWithTag.tag == "Door")
            {
                childWithTag.SetActive(true);
            }
        }
        Instantiate(roomToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void GenerateRoom(Vector3 _roomSpawnPos)
    {
        GameObject roomToSpawn = rooms[Random.Range(0, rooms.Length)];
        GameObject roomInstance = Instantiate(roomToSpawn, _roomSpawnPos, Quaternion.identity);
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
        GameObject doorPlayerEntered = FindDoorPlayerEntered(doors);
        Debug.Log(doorPlayerEntered);
        Debug.Log(doorPlayerEntered.transform.position);
        doorPlayerEntered.SetActive(true);
        List<GameObject> doorsNotSpawned = new List<GameObject>(doors);
        doorsNotSpawned.RemoveAll(d => d.activeSelf);
        numberOfDoorsToSpawn--;
        SpawnRandomDoor(doorsNotSpawned, numberOfDoorsToSpawn);
        numberOfRoomsToGenerate--;   
    }

    GameObject FindDoorPlayerEntered(List<GameObject> _doorsInRoom)
    {
        GameObject doorPlayerEntered = null;
        float distanceFromPlayer = 0;
        float smallestDistance = 999999;
        for (int i = 0; i < _doorsInRoom.Count; i++)
        {
            distanceFromPlayer = Vector3.Distance(_doorsInRoom[i].transform.position, player.transform.position);
            if (distanceFromPlayer < smallestDistance)
            {
                smallestDistance = distanceFromPlayer;
                doorPlayerEntered = _doorsInRoom[i];
            }
        }
        return doorPlayerEntered;
    }

    void SpawnRandomDoor(List<GameObject> _doorsNotSpawned, int _numberOfDoorsToSpawn)
    {
        int doorToSpawn = Random.Range(0, _doorsNotSpawned.Count);
        Debug.Log(doorToSpawn);
        Debug.Log(_doorsNotSpawned.Count);
        _doorsNotSpawned[doorToSpawn].SetActive(true);
        _doorsNotSpawned.Remove(_doorsNotSpawned[doorToSpawn]);
        _numberOfDoorsToSpawn--;
        if (_numberOfDoorsToSpawn > 0)
        {
            SpawnRandomDoor(_doorsNotSpawned, _numberOfDoorsToSpawn);
        }
    }
}
