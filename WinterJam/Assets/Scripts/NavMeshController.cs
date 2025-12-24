using Unity.AI.Navigation;
using UnityEngine;

// Attach this script to a TerrainGenerator GameObject to build a NavMesh at runtime
public class NavMeshController : MonoBehaviour
{
    NavMeshSurface meshSurface;
    void Start()
    {
        meshSurface = gameObject.AddComponent<NavMeshSurface>();
        meshSurface.BuildNavMesh();
    }

    void Update()
    {
        
    }
}
