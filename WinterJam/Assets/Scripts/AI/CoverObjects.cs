using System.Collections.Generic;
using UnityEngine;

// Put this script on a TerrainGenerator game object or dynamically allocate it to track cover objects in a room
public class CoverObjects : MonoBehaviour
{
    public Dictionary<GameObject, CoverState> coverObjects;

    void Start()
    {
        coverObjects = new Dictionary<GameObject, CoverState>();
        InitializeObjectList();
    }
    void InitializeObjectList()
    {
        foreach (Transform childTransform in transform)
        {
            if(childTransform.CompareTag("Cover"))
            {
                coverObjects.Add(childTransform.gameObject, CoverState.Empty);
            }
        }
    }
}
