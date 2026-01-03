using UnityEngine;


// Place on Character
// Feed IceBlock Prefab and prefered materials
// Call func from character on death
public class IceEffect : MonoBehaviour
{
    [SerializeField] private GameObject iceBlock;
    [SerializeField] private Material[] iceMats;

    private GameObject iceBlockInstance;

    public void SpawnIceBlock()
    {
        if (iceBlockInstance != null) return; // stops double spawn

        iceBlockInstance = Instantiate(iceBlock, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        iceBlockInstance.transform.SetParent(null);
        ApplyMat();
    }

    // Helpers
    void ApplyMat()
    {
        MeshRenderer renderer = iceBlock.GetComponent<MeshRenderer>();
        renderer.material = iceMats[Random.Range(0, iceMats.Length)];
    }
}
