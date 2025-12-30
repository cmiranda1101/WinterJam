using System.Collections;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour
{
    [SerializeField] GameObject terrainGenerator;
    TerrainGenerator terrainGeneratorScript;

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
        //Divide by 2 to get half the size since the terrain is centered at (0,0)
        xBound = terrainGeneratorScript.xSize/2 - xBoundOffset;
        zBound = terrainGeneratorScript.zSize/2 - zBoundOffset;
        RandomlyPlaceEnemies();
    }
    
    public void RandomlyPlaceEnemies()
    {
        Vector3 objectSpawnPos;
        for (int i = 0; i < objectDensity; i++)
        {
            //Generate random position within bounds of terrain adjusted for centering
            float sampleX = Random.Range(-xBound, xBound);
            float sampleZ = Random.Range(-zBoundOffset, zBound);
            Vector3 randomPos = new Vector3(transform.parent.position.x + sampleX, maxHeight, transform.parent.position.z + sampleZ);
            Debug.Log(randomPos);
            int objectToPlaceIndex = Random.Range(0, objectsToPlace.Length);
            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit) && hit.transform.gameObject.tag != "CantSpawnOn") //make sure all objects spawned using object spawn have this tag to avoid them spawning on each other
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
            instance.transform.SetParent(terrainGenerator.transform);
        }
    }
}
