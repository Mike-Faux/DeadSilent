
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

    public IEnumerator Reload()
    {
        if (isReloading) yield break; // Prevent multiple reloads at the same time

        isReloading = true;
        

        // Simulate reload time
        yield return new WaitForSeconds(3); // Adjust the reload time as needed

        Ammo = ammoMax;
        isReloading = false;
        
    }

    IEnumerator Shoot(float time)
    {
        isShooting = true;
        Ammo--; // Decrement ammo count on shot

        float randomChance = Random.Range(0f, 100f); // Generate a random number between 0 and 100

        // Check if the shot will miss based on hitChance
        if (randomChance > hitChance)
        {
            // The shot will miss
            Debug.Log("Shot missed due to randomness.");
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
                    Vector3 targetPosition = player.transform.position; // Capture the player's position at the time of firing
                    Vector3 direction = (targetPosition - firePoint.position).normalized; // Calculate direction based on the captured position
                    EnemyBullet enemyBulletScript = bullet.GetComponent<EnemyBullet>();
                    if (enemyBulletScript != null)
                    {
                        enemyBulletScript.speed = Stats.BulletSpeed;
                        enemyBulletScript.damage = Stats.Damage;
                        enemyBulletScript.maxRange = Stats.MaxRange;
                        enemyBulletScript.SetDirection(direction); // Set the direction once, based on the initial calculation
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
