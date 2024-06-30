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
    [SerializeField] public float FireRate;
    [SerializeField] public AudioClip reloadClip;


    private AudioSource audioSource;
    public int Ammo;
    public int ammoMax;
    public LayerMask Enemy;
    bool isShooting;
    public bool isReloading;
    private bool fireEnemyBullet = false;


    //aiming
    public Vector3 normalLocalPosition;
    public Vector3 aimLocalPosition;

    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;
    public float swaySmoothness = 4f;

    public float aimSmoothing = 10f;


    
    //recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //if we do not want random recoil, we can use a fixed recoil pattern
    public Vector2 recoilPattern;



    private BulletType currentBulletType = BulletType.PlayerBullet;



    private void Update()
    {
        DetermineAim();
        ApplySway(); 
    }
    private void ApplySway()
    {
        bool isAiming = Input.GetButton("Fire2"); // Assuming "Fire2" is your aiming input
        float swayFactor = isAiming ? 0.5f : 1f; // Reduce sway by half when aiming

        float mouseX = Input.GetAxis("Mouse X") * swayAmount * swayFactor;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount * swayFactor;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        Vector3 swayOffset = new Vector3(mouseX, mouseY, 0);
        transform.localPosition += swayOffset * Time.deltaTime * swaySmoothness;
    }

    public void DetermineAim()
    {
        Vector3 target = Input.GetButton("Fire2") ? aimLocalPosition : normalLocalPosition;
        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
        transform.localPosition = desiredPosition;
    }
    public void SetBulletType(BulletType bulletType)
    {
        currentBulletType = bulletType;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            audioSource.Play();
            DetermineRecoil();
            StartCoroutine(Shoot(Stats.FireRate));
        }
        
    }

    public void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;

        if(randomizeRecoil)
        {
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);

            transform.localPosition += Vector3.forward * recoil.x;
            transform.localPosition += Vector3.up * recoil.y;


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
        audioSource.PlayOneShot(reloadClip);
        if (useAmmo)
        {
            int ammoInInventory = GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type);
            if (ammoInInventory <= 0)
            {
                Debug.Log("Not enough ammo in inventory to reload.");
                return;
            }
            StartCoroutine(Reload(Stats.ReloadTime, true)); // Updated to call the correctly named coroutine
        }
        else
        {
            StartCoroutine(Reload(Stats.ReloadTime)); // Updated to call the correctly named coroutine
        }
    }

    IEnumerator Shoot(float time)
    {
        isShooting = true;

        Vector3 targetPoint = Vector3.zero;
        Vector3 direction;

        if (!fireEnemyBullet) 
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 5));
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
        GameObject bulletObject = Instantiate(bulletPrefab, FirePos.position + direction * 1f, Quaternion.LookRotation(direction));

        // Reset the flag if necessary
        fireEnemyBullet = false; // Reset after use 

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
                FireRate = Stats.FireRate;
            }
            else
            {
                Debug.LogError("Bullet component not found on the instantiated object.");
            }
        }
        else 
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

        if (useAmmo)
        {
            int reserveAmmo = GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type);
            int ammoNeeded = Stats.Ammo_Capacity - Ammo;
            int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

            if (ammoToReload > 0)
            {
                Ammo += ammoToReload; // Update the current ammo
                GameManager.Instance.playerScript.inventory.RemoveItems(Stats.Ammo_Type, ammoToReload); // Deduct the reloaded ammo from the inventory
            }
        }
        else
        {
            // If not using ammo from the inventory, simply refill the magazine to its capacity
            int ammoNeeded = Stats.Ammo_Capacity - Ammo;
            Ammo = Stats.Ammo_Capacity; // Fully reload without using inventory ammo
            
             GameManager.Instance.playerScript.inventory.RemoveItems(Stats.Ammo_Type, ammoNeeded);
        }

        isReloading = false;

        // Update the UI with the new ammo counts
        GameManager.Instance.UpdateAmmoCount(Ammo, GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type));
    }
}
