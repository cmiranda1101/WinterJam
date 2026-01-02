using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] GameObject snowBallPrefab;
    [SerializeField] float muzzleVelocity;
    [SerializeField] private Transform muzzle;
    [SerializeField] public float shootTimer;
    float timer;
    float destroyTimer = 3f;

    void Start()
    {
    }
    
    void Update()
    {
        timer += Time.deltaTime;
    }

    public void Shoot()
    {
        if(timer < shootTimer) return;
        GameObject projectile = Instantiate(snowBallPrefab, muzzle.position, muzzle.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(muzzle.forward * muzzleVelocity);
        StartCoroutine(DestroyProjectile(projectile));
        timer = 0f;
    }

    public IEnumerator DestroyProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(destroyTimer);
        if(projectile != null)
        Destroy(projectile);
    }
}
