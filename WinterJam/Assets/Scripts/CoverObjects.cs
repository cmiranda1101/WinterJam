using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// Put this script on a TerrainGenerator game object or dynamically allocate it to track cover objects in a room
public class CoverObjects : MonoBehaviour
{
    public List<GameObject> coverObjects;

    NavMeshController navMeshController;

    private async void Start()
    {
        navMeshController = GetComponent<NavMeshController>();
        coverObjects = new List<GameObject>();
        await OnStart();
    }
    private async Task OnStart()
    {
        while (navMeshController.isBaking)
        {
            await Task.Yield();
        }
        foreach (Transform childTransform in transform)
        {
            if(childTransform.CompareTag("Cover"))
            {
                coverObjects.Add(childTransform.gameObject);
                await Task.Yield();
            }
        }
    }
}
