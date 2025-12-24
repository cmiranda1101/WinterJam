using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] GameObject terrainGenerator;
    TerrainGenerator terrainGeneratorScript;

    [SerializeField] GameObject objectToPlace;
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
        for (int i = 0; i < objectDensity; i++)
        {
            float sampleX = Random.Range(xBoundOffset, xBound);
            float sampleZ = Random.Range(zBoundOffset, zBound);
            Vector3 randomPos = new Vector3(sampleX, maxHeight, sampleZ);
            Debug.Log(randomPos);
            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit, maxHeight + 1))
            {
                Debug.Log(hit.point);
                objectSpawnPos = hit.point;
            }
            else
            {
                continue;
            }
            Instantiate(objectToPlace, objectSpawnPos, Quaternion.identity);
        }
    }
}
