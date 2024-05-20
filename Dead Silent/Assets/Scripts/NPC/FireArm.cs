using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour, IWeapon
{

    [SerializeField] Transform FirePos;
    [SerializeField] int Damage;
    [SerializeField] int BulletSpeed;
    [SerializeField] float FireRate;

    public int Ammo;
    public int Ammo_Capacity;
    [SerializeField] float ReloadTime;

    [SerializeField] GameObject Bullet;

    public bool Silenced;
    [SerializeField] bool InfiniteAmmo = false;

    bool isShooting;
    bool isReloading;

    public void Attack()
    {
        if (Ammo <= 0 && !InfiniteAmmo) return;
        if(isReloading) return;

        if(!isShooting)
        {
            StartCoroutine(Shoot(FireRate));
        }
    }

    public void Reload()
    {
        if(!isReloading)
        {
            StartCoroutine(Reload(ReloadTime));
        }
    }

    public void ChangeAmmo(int amount)
    {
        Ammo += amount;

        if (Ammo > Ammo_Capacity)
        {
            Ammo = Ammo_Capacity;
        }
    }

    IEnumerator Shoot(float time)
    {
        Ammo--;
        isShooting = true;
        Bullet bullet = Instantiate(Bullet, FirePos.transform.position, transform.rotation).GetComponent<Bullet>();
        bullet.speed = BulletSpeed;
        bullet.damage = Damage;
        yield return new WaitForSeconds(time);
        isShooting = false;
    }
    IEnumerator Reload(float time)
    {
        isReloading = true;
        Ammo = 0;
        yield return new WaitForSeconds(time);
        Ammo = Ammo_Capacity;
        isReloading = false;
    }
}
