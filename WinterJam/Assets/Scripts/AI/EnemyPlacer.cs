using System.Collections;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour
{
    [SerializeField] GameObject terrainGenerator;
    TerrainGenerator terrainGeneratorScript;
    NavMeshController navMeshController;

    [SerializeField] GameObject[] objectsToPlace;
    [SerializeField] int objectDensity;

    [SerializeField] int maxHeight;
    [SerializeField] int xBoundOffset; //The minimum distance an object must be from the edge to spawn
    [SerializeField] int zBoundOffset;
    int xBound;
    int zBound;
    
    void Start()
    {
        terrainGeneratorScript = terrainGenerator.GetComponent<TerrainGenerator>();
        navMeshController = terrainGenerator.GetComponent<NavMeshController>();
        xBound = terrainGeneratorScript.xSize - xBoundOffset;
        zBound = terrainGeneratorScript.zSize - zBoundOffset;
        StartCoroutine(RandomlyPlaceEnemies());
    }

    // Coroutine to randomly place enemies after the NavMesh is baked
    public IEnumerator RandomlyPlaceEnemies()
    {
        // Wait until the NavMesh is done baking
        while (navMeshController.isBaking)
        {
            yield return null;
        }
        Vector3 objectSpawnPos;
        for (int i = 0; i < objectDensity; i++)
        {
            float sampleX = Random.Range(xBoundOffset, xBound);
            float sampleZ = Random.Range(zBoundOffset, zBound);
            Vector3 randomPos = new Vector3(sampleX, maxHeight, sampleZ);
            int objectToPlaceIndex = Random.Range(0, objectsToPlace.Length);
            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit, maxHeight + 1) && hit.transform.gameObject.tag != "CantSpawnOn") //make sure all objects spawned using object spawn have this tag to avoid them spawning on each other
            {
                objectSpawnPos = new Vector3(hit.point.x, hit.point.y + 0.8f, hit.point.z);
            }
            else
            {
                continue;
            }
            GameObject instance = Instantiate(objectsToPlace[objectToPlaceIndex], objectSpawnPos, Quaternion.identity);
            instance.tag = "Enemy";
            // Set the enemy as a child of the terrain generator for organization
            // This is also used by the enemy controller to know when the NavMesh is ready
            instance.transform.SetParent(terrainGenerator.transform);
        }
        yield break;
    }
}
