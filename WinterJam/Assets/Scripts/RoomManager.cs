using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] GameObject room;
    [SerializeField] GameObject[] doors;
    public GameObject northDoor;
    public GameObject southDoor;
    public GameObject westDoor;
    public GameObject eastDoor;

    GameObject nextRoom;
    public Vector3 newRoomSpawnPos;

    public void CheckRoomConnections()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            GameObject door = doors[i];
            newRoomSpawnPos = door.transform.position - (door.transform.TransformDirection(Vector3.forward) * 30);
            nextRoom = CheckIfRoomExists(newRoomSpawnPos, doors[i]);
            if (nextRoom != null)
            {
                door.GetComponent<Door>().neighboringRoom = nextRoom;
            }
        }
    }

    GameObject CheckIfRoomExists(Vector3 _posToCheck, GameObject _doorToCheck)
    {
        GameObject nextRoom = null;
        _posToCheck.y = 20;
        RaycastHit hit;
        Debug.Log(_posToCheck);
        if (Physics.Raycast(_posToCheck, Vector3.down, out hit))
        {
            if (hit.collider != null)
            {
                nextRoom = hit.collider.gameObject.transform.parent.gameObject; //Gets the entire room, not just an object in it
                _doorToCheck.GetComponent<Door>().neighboringRoom = nextRoom;
            }
        }
        return nextRoom;
    }

    public void CheckIfDoorIsInvalid(GameObject _nextRoomInstance, GameObject _doorToCheck)
    {
        Door doorScript = _doorToCheck.GetComponent<Door>();
        if (doorScript.neighboringRoom == null)
        {
            return;
        }
        Debug.Log(doorScript.direction);
        List<GameObject> doorsInNextRoom = GetDoorsFromNeighboringRoom(doorScript.neighboringRoom);
        GameObject oppositeDoor = null;
        bool isOppositeDoorActive = false;
        switch(doorScript.direction)
        {
            case DoorDirection.North:
                oppositeDoor = GetDoorByDirection(doorsInNextRoom, DoorDirection.South);
                if (oppositeDoor.activeSelf)
                {
                    isOppositeDoorActive = true;
                }
                break;

            case DoorDirection.South:
                oppositeDoor = GetDoorByDirection(doorsInNextRoom, DoorDirection.North);
                if (oppositeDoor.activeSelf)
                {
                    isOppositeDoorActive = true;
                }
                break;

            case DoorDirection.West:
                oppositeDoor = GetDoorByDirection(doorsInNextRoom, DoorDirection.East);
                if (oppositeDoor.activeSelf)
                {
                    isOppositeDoorActive = true;
                }
                break;

            case DoorDirection.East:
                oppositeDoor = GetDoorByDirection(doorsInNextRoom, DoorDirection.West);
                if (oppositeDoor.activeSelf)
                {
                    isOppositeDoorActive = true;
                }
                break;
        }
        if (isOppositeDoorActive)
        {
            doorScript.isDoorEssential = true;
        }
        else
        {
            doorScript.isDoorInvalid = true;
        }
    }

    List<GameObject> GetDoorsFromNeighboringRoom(GameObject _nextRoomInstance)
    {

        List<GameObject> doors = new List<GameObject>();
        for (int i = 0; i < _nextRoomInstance.transform.childCount; i++)
        {
            if (_nextRoomInstance.transform.GetChild(i).CompareTag("Door"))
            {
                doors.Add(_nextRoomInstance.transform.GetChild(i).gameObject);
            }
        }
        return doors;
    }

    GameObject GetDoorByDirection(List<GameObject> _doors, DoorDirection _direction)
    {
        GameObject doorToReturn = null;
        for (int i = 0; i < _doors.Count; i++)
        {
            Door doorScript = _doors[i].transform.GetChild(1).GetComponent<Door>();
            if (doorScript.direction == _direction)
            {
                doorToReturn = _doors[i];
                break;
            }
        }
        return doorToReturn;
    }
}
