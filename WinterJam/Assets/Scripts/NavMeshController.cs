using System.Collections;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEngine;

// Attach this script to a TerrainGenerator GameObject to build a NavMesh at runtime
public class NavMeshController : MonoBehaviour
{
    NavMeshSurface meshSurface;
    public bool isBaking = true;

    void Start()
    {
        meshSurface = gameObject.AddComponent<NavMeshSurface>();
        StartCoroutine(BakeNavMesh());
    }

    // Coroutine to bake the NavMesh asynchronously to avoid blocking the main thread and signal when baking is complete
    // The isBaking flag can be used by other scripts to check if the NavMesh is ready
    public IEnumerator BakeNavMesh()
    {
        meshSurface.navMeshData = new NavMeshData();
        meshSurface.AddData();
        AsyncOperation operation = meshSurface.UpdateNavMesh(meshSurface.navMeshData);
        while (!operation.isDone)
        {
            yield return null;
        }
        isBaking = false;
        yield return operation;
    }
}
