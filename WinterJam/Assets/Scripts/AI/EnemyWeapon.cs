using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] GameObject snowBallPrefab;
    [SerializeField] float muzzleVelocity;
    [SerializeField] private Transform muzzle;
    [SerializeField] public float shootTimer;
    [SerializeField] public float maxRange;
    float timer;
    float destroyTimer = 3f;

    void Start()
    {
    }
    
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void Shoot(Transform player)
    {
        if(timer < shootTimer) return;
        RaycastHit hit;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        if (Physics.Raycast(muzzle.position, directionToPlayer, out hit, maxRange))
        {
            if(hit.collider.CompareTag("Player"))
            {
                GameObject projectile = Instantiate(snowBallPrefab, muzzle.position, muzzle.rotation);
                projectile.GetComponent<Rigidbody>().AddForce(directionToPlayer * muzzleVelocity);
                StartCoroutine(DestroyProjectile(projectile));
                timer = 0f;
            }
        }
    }

    public IEnumerator DestroyProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(destroyTimer);
        if(projectile != null)
        Destroy(projectile);
    }
}
