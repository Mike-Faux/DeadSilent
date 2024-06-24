
using UnityEngine;
using System.Collections;
public class EnemyFirearm : MonoBehaviour, IWeapon
{
    public GameObject bulletPrefab; 
    public Transform firePoint; 
    public float fireRate = 1f; 
    public WeaponStats Stats;
    public ParticleSystem hitEffect;
    public float hitChance = 75f;
    public int Ammo;
    public int ammoMax;
    public LayerMask Enemy;
    bool isShooting;
    bool isReloading;
    [SerializeField] private GameObject missedShotEffect;

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
        float randomChance = Random.Range(0f, 100f); // Generate a random number between 0 and 100

        // Check if hitChance is lower or equal to 30%, ensuring the shot will miss
        if (hitChance <= 80f || randomChance > hitChance)
        {
            // The shot will miss
            Debug.Log("Shot missed due to low hit chance or randomness.");
            // Optionally, handle the miss case here (e.g., instantiate a bullet that visually misses, play a miss sound or animation)
            HandleMissedShot();
        }
        else
        {
            // The shot hits
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
        }

        yield return new WaitForSeconds(time); // Wait for the specified time before allowing another shot
        isShooting = false;
    }
    void HandleMissedShot()
    {
        if (missedShotEffect != null)
        {
            Instantiate(missedShotEffect, firePoint.position, Quaternion.identity);
        }
    }
}
