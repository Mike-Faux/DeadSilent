
using UnityEngine;
using System.Collections;
public class EnemyFirearm : MonoBehaviour, IWeapon
{
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public float fireRate = 1f; 
    public WeaponStats Stats;
    public ParticleSystem hitEffect;

    public int Ammo;
    public int ammoMax;
    public LayerMask Enemy;
    bool isShooting;
    bool isReloading;

    private float nextFireTime = 0f;

    public void Attack()
    {
        if (Ammo <= 0 || isReloading || isShooting) return;

        if (Time.time >= nextFireTime)
        {
            StartCoroutine(Shoot(Stats.FireRate));
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Update()
    {
        
    }

    IEnumerator Shoot(float time)
    {
        isShooting = true;

        if (bulletPrefab != null && firePoint != null)
        {
            
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
           
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 direction = (player.transform.position - firePoint.position).normalized;
                
                EnemyBullet enemyBulletScript = bullet.GetComponent<EnemyBullet>();
                if (enemyBulletScript != null)
                {
                    enemyBulletScript.speed = Stats.BulletSpeed;
                    enemyBulletScript.damage = Stats.Damage;
                    enemyBulletScript.maxRange = Stats.MaxRange;
                    enemyBulletScript.SetDirection(direction);
                }
            }
        }

        yield return new WaitForSeconds(time); 

        isShooting = false;
    }
}
