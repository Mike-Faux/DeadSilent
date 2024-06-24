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
    public int Ammo;
    public int ammoMax;
    public LayerMask Enemy;
    bool isShooting;
    bool isReloading;

    private void Start()
    {
        
    }

    public void Attack()
    {
        if (Ammo <= 0) return;
        if(isReloading) return;
       
        if (!isShooting)
        {
            //Debug.Log("shooting");
            StartCoroutine(Shoot(Stats.FireRate));
        }
    }

    public void Reload(bool useAmmo = false)
    {
        if (isReloading || Ammo == Stats.Ammo_Capacity) return; // Also check if ammo is already full

        if (useAmmo)
        {
            int ammoInInventory = GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type);
            if (ammoInInventory <= 0)
            {
                // Provide feedback to the player about lack of ammo
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
       
      
        
       Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPoint = hit.point;
        }else
        {
            targetPoint = ray.GetPoint(1000);
        }
        
        Vector3 direction = (targetPoint - FirePos.position).normalized;
        Ammo--;
      
        Bullet bullet = Instantiate(Bullet, FirePos.position, Quaternion.identity).GetComponent<Bullet>();
      
        bullet.damage = Stats.Damage;
        bullet.maxRange = Stats.MaxRange;
        bullet.hitEffect = hitEffect;
        bullet.SetDirection(direction);
        yield return new WaitForSeconds(time);
        isShooting = false;
    }

    private IEnumerator Reload(float reloadTime, bool useAmmo = false)
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime); // Simulate reload delay

        // Assuming you have a method in your inventory system to get the count of a specific item type
        int reserveAmmo = GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type);
        int ammoNeeded = Stats.Ammo_Capacity - Ammo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        Ammo += ammoToReload; // Update the current ammo

        // Assuming you have a method in your inventory system to remove items
        GameManager.Instance.playerScript.inventory.RemoveItems(Stats.Ammo_Type, ammoToReload);

        isReloading = false;

        // Update the UI with the new ammo counts, fetching the updated reserve ammo count again
        GameManager.Instance.UpdateAmmoCount(Ammo, GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type));
    }
}
