using UnityEngine;

public class ObjectPlacer : MonoBehaviour
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrainGeneratorScript = terrainGenerator.GetComponent<TerrainGenerator>();
        xBound = terrainGeneratorScript.xSize - xBoundOffset;
        zBound = terrainGeneratorScript.zSize - zBoundOffset;
        RandomlyPlaceObjects();
    }


    void RandomlyPlaceObjects()
    {
        Vector3 objectSpawnPos;
        int objectsSpawned = 0;
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
                objectsSpawned++;
            }
            else
            {
                continue;
            }
            Instantiate(objectsToPlace[objectToPlaceIndex], objectSpawnPos, Quaternion.identity);
        }
        Debug.Log(objectsSpawned);
    }
}
