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
        if (isReloading) return;

        if(useAmmo)
        {
            if (GameManager.Instance.playerScript.inventory.GetItemCount(Stats.Ammo_Type) <= 0) return;
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
      
        Bullet bullet = Instantiate( Bullet, FirePos.transform.position, transform.rotation).GetComponent<Bullet>();
      
        bullet.damage = Stats.Damage;
        bullet.maxRange = Stats.MaxRange;
        bullet.hitEffect = hitEffect;
        yield return new WaitForSeconds(time);
        isShooting = false;
    }

    IEnumerator Reload(float time, bool useAmmo = false)
    {
        isReloading = true;
        int remainingAmmo = Ammo;
        Ammo = 0;
        yield return new WaitForSeconds(time);
        Ammo = Stats.Ammo_Capacity;

        if (useAmmo)
        {
            Ammo = GameManager.Instance.playerScript.inventory.RemoveItems(Stats.Ammo_Type, Stats.Ammo_Capacity - remainingAmmo) + remainingAmmo;
        }

        isReloading = false;
    }
}
