using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour, IWeapon
{

    public WeaponStats Stats;
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

        if(!isShooting)
        {
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
        Ammo--;
        GameManager.Instance.UpdateAmmoCount(Ammo, Stats.Ammo_Capacity);
        isShooting = true;
        Bullet bullet = Instantiate(Bullet, FirePos.transform.position, transform.rotation).GetComponent<Bullet>();
        bullet.speed = Stats.BulletSpeed;
        bullet.damage = Stats.Damage;
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
