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
    [SerializeField] bool InfiniteAmmo = false;
    public int Ammo;

    bool isShooting;
    bool isReloading;

    private void Start()
    {
        GameManager.Instance.UpdateAmmoCount(Ammo, Stats.Ammo_Capacity);
    }

    public void Attack()
    {
        if (Ammo <= 0 && !InfiniteAmmo) return;
        if(isReloading) return;
       
        if (!isShooting)
        {
            Debug.Log("shooting");
            StartCoroutine(Shoot(Stats.FireRate));
            

            

        }
    }

    public void Reload()
    {
        if(!isReloading)
        {
            StartCoroutine(Reload(Stats.ReloadTime));
        }

    }

    public void ChangeAmmo(int amount)
    {
        Ammo += amount;

        if (Ammo > Stats.Ammo_Capacity)
        {
            Ammo = Stats.Ammo_Capacity;
        }
        GameManager.Instance.UpdateAmmoCount(Ammo, Stats.Ammo_Capacity);
    }

    IEnumerator Shoot(float time)
    {
      
        GameManager.Instance.LastKnownPosition = GameManager.Instance.Player.transform.position;
        Collider[] units = Physics.OverlapSphere(transform.position,  Stats.SoundRadius, LayerMask.GetMask("Characters"));
        
        //for(int i = 0; i < units.Length; i++)
        //{
        //    if (units[i].TryGetComponent(out EnemyAI ai))
        //    {
        //        ai.Alert();
        //    }
        //}
        
        Ammo--;
        isShooting = true;
        Bullet bullet = Instantiate( Bullet, FirePos.transform.position, transform.rotation).GetComponent<Bullet>();
        bullet.speed = Stats.BulletSpeed;
        bullet.damage = Stats.Damage;
        bullet.maxRange = Stats.MaxRange;
        bullet.hitEffect = hitEffect;
        yield return new WaitForSeconds(time);
        isShooting = false;
    }
    IEnumerator Reload(float time)
    {
        isReloading = true;
        Ammo = 0;
        yield return new WaitForSeconds(time);
        Ammo = Stats.Ammo_Capacity;
        GameManager.Instance.UpdateAmmoCount(Ammo, Stats.Ammo_Capacity);
        isReloading = false;
    }
}
