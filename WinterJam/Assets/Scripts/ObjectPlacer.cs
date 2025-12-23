using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] GameObject terrainGenerator;
    TerrainGenerator terrainGeneratorScript;

    [SerializeField] GameObject objectToPlace;
    [SerializeField] int objectDensity;

    [SerializeField] int maxHeight;
    [SerializeField] int minHeight;
    int xBound;
    int zBound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        terrainGeneratorScript = terrainGenerator.GetComponent<TerrainGenerator>();
        xBound = terrainGeneratorScript.xSize;
        zBound = terrainGeneratorScript.zSize;
        Debug.Log(xBound);
        Debug.Log(zBound);
        RandomlyPlaceObjects();
    }


    void RandomlyPlaceObjects()
    {
        Vector3 objectSpawnPos;
        for (int i = 0; i < objectDensity; i++)
        {
            float sampleX = Random.Range(0, xBound);
            float sampleZ = Random.Range(0, zBound);
            Vector3 randomPos = new Vector3(sampleX, maxHeight, sampleZ);
            Debug.Log(randomPos);
            RaycastHit hit;
            if (Physics.Raycast(randomPos, Vector3.down, out hit, maxHeight))
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
