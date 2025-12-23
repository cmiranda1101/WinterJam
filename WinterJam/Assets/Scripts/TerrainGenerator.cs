using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    Mesh terrain;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public float xOffset = 100f;
    public float zOffset = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xOffset = Random.Range(0f, 999999f);
        zOffset = Random.Range(0f, 999999f);
        terrain = new Mesh();
        GetComponent<MeshFilter>().mesh = terrain;
        GenerateTerrain();
        UpdateTerrainMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTerrain()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.25f + xOffset, z * 0.25f + zOffset) * 2;
                vertices[i] = new Vector3(x, y, z); //if you want flat terrain swap y to 0
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateTerrainMesh()
    {
        terrain.Clear();
        terrain.vertices = vertices;
        terrain.triangles = triangles;
        terrain.RecalculateNormals();
    }

    //debug function, remove before final release
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
