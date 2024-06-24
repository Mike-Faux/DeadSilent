using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FireArm : MonoBehaviour, IWeapon
{
    
    public WeaponStats Stats;
    public ParticleSystem hitEffect;
    [SerializeField] Transform FirePos;
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject EnemyBullet;
    public int Ammo;
    public int ammoMax;
    public LayerMask Enemy;
    bool isShooting;
    bool isReloading;
    private bool fireEnemyBullet = false;

    private BulletType currentBulletType = BulletType.PlayerBullet;


    public void SetBulletType(BulletType bulletType)
    {
        currentBulletType = bulletType;
    }

    private void Start()
    {
        
    }

    public void TriggerAttack()
    {
       fireEnemyBullet = true; 
        StartCoroutine(Shoot(Stats.FireRate));
    }

    public void Attack()
    {
        if (Ammo <= 0) return;
        if(isReloading) return;
       
        if (!isShooting)
        {
            
            StartCoroutine(Shoot(Stats.FireRate));
        }
    }
    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet
    }
    public void Reload(bool useAmmo = false)
    {
        if (isReloading || Ammo == Stats.Ammo_Capacity) return; // Also check if ammo is already full

        if (useAmmo)
        {
            int ammoInInventory = GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type);
            if (ammoInInventory <= 0)
            {
                
                Debug.Log("Not enough ammo in inventory to reload.");
                return;
            }
            StartCoroutine(Reload(Stats.ReloadTime, true));
        }
        else
        {
            StartCoroutine(Reload(Stats.ReloadTime));
        }
    }

    IEnumerator Shoot(float time)
    {
        isShooting = true;

        Vector3 targetPoint = Vector3.zero;
        Vector3 direction;

        if (!fireEnemyBullet) 
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(1000); 
            }
        }
        else 
        {
            
            GameObject player = GameObject.FindGameObjectWithTag("Player"); 
            if (player != null)
            {
                targetPoint = player.transform.position;
               
            }
            else
            {
                Debug.LogError("Player not found for enemy aiming.");
                yield break;
            }
        }

        direction = (targetPoint - FirePos.position).normalized;
        Ammo--;

        // Choose the bullet type based on the flag
        GameObject bulletPrefab = fireEnemyBullet ? EnemyBullet : Bullet;
        GameObject bulletObject = Instantiate(bulletPrefab, FirePos.position, Quaternion.LookRotation(direction));

        // Reset the flag if necessary
        fireEnemyBullet = false; // Reset after use if you're toggling this per shot

        // Set bullet properties based on type
        if (bulletPrefab == Bullet)
        {
            Bullet bullet = bulletObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.damage = Stats.Damage;
                bullet.maxRange = Stats.MaxRange;
                bullet.hitEffect = hitEffect;
                bullet.SetDirection(direction);
            }
            else
            {
                Debug.LogError("Bullet component not found on the instantiated object.");
            }
        }
        else // Assuming the enemy bullet has similar properties/methods to set
        {
            EnemyBullet enemyBullet = bulletObject.GetComponent<EnemyBullet>();
            if (enemyBullet != null)
            {
                enemyBullet.damage = Stats.Damage;
                enemyBullet.maxRange = Stats.MaxRange;
                enemyBullet.hitEffect = hitEffect;
                enemyBullet.SetDirection(direction);
               
            }
        }

        yield return new WaitForSeconds(time);
        isShooting = false;
    }
    public void ShootAtTarget(Vector3 targetPosition)
    {
        Bullet bullet = Instantiate(Bullet, FirePos.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.InitializeWithTarget(targetPosition);
    }
    private IEnumerator Reload(float reloadTime, bool useAmmo = false)
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime); // Simulate reload delay

        int reserveAmmo = GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type);
        int ammoNeeded = Stats.Ammo_Capacity - Ammo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        Ammo += ammoToReload; // Update the current ammo

        GameManager.Instance.playerScript.inventory.RemoveItems(Stats.Ammo_Type, ammoToReload);

        isReloading = false;

        // Update the UI with the new ammo counts, fetching the updated reserve ammo count again
        GameManager.Instance.UpdateAmmoCount(Ammo, GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type));
    }
}
