using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] rooms;
    public int numberOfRoomsToGenerate;

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
        Instantiate(roomToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
        numberOfRoomsToGenerate--;
    }

    public void GenerateRoom(Vector3 _roomSpawnPos)
    {
        GameObject roomToSpawn = rooms[Random.Range(0, rooms.Length)];
        GameObject childWithTag;
        List<GameObject> doors = new List<GameObject>();
        for (int i = 0; i < roomToSpawn.transform.childCount; i++)
        {
            childWithTag = roomToSpawn.transform.GetChild(i).gameObject;
            if (childWithTag.tag == "Door")
            {
                doors.Add(childWithTag);
                childWithTag.SetActive(false);
            }
        }
        int numberOfDoorsToSpawn = Random.Range(2, doors.Count);
        GameObject doorPlayerEntered = FindDoorPlayerEntered(doors);
        doorPlayerEntered.SetActive(true);
        List<GameObject> doorsNotSpawned = doors;
        for (int i = 0; i < doorsNotSpawned.Count; i++)
        {
            if (doorsNotSpawned[i] == doorPlayerEntered)
            {
                doorsNotSpawned.Remove(doorsNotSpawned[i]);
            }
        }
        SpawnRandomDoor(doorsNotSpawned, numberOfDoorsToSpawn);
        Instantiate(roomToSpawn, _roomSpawnPos, Quaternion.identity);
        numberOfRoomsToGenerate--;   
    }

    GameObject FindDoorPlayerEntered(List<GameObject> _doorsInRoom)
    {
        GameObject doorPlayerEntered = new GameObject();
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
        int doorToSpawn = Random.Range(1, _doorsNotSpawned.Count);
        Debug.Log(doorToSpawn);
        Debug.Log(_doorsNotSpawned.Count);
        _doorsNotSpawned[doorToSpawn - 1].SetActive(true);
        _doorsNotSpawned.Remove(_doorsNotSpawned[doorToSpawn]);
        _numberOfDoorsToSpawn--;
        if (_numberOfDoorsToSpawn > 0)
        {
            SpawnRandomDoor(_doorsNotSpawned, _numberOfDoorsToSpawn);
        }
    }
}
