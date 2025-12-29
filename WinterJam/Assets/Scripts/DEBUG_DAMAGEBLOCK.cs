using UnityEngine;

public class DEBUG_DAMAGEBLOCK : MonoBehaviour
{
    [SerializeField] float testDamage;

    public Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponentInParent<IDamage>();
        if (dmg != null)
        {
            dmg.TakeDamage(testDamage);
        }
    }
}
