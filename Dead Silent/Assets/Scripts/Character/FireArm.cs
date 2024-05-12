using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour, Weapon
{

    [SerializeField] Transform FirePos;
    [SerializeField] int Damage;
    [SerializeField] int BulletSpeed;
    [SerializeField] float FireRate;

    [SerializeField] GameObject Bullet;

    bool isShooting;

    public void Attack()
    {
        if(!isShooting)
        {
            StartCoroutine(Shoot(FireRate));
        }
    }

    IEnumerator Shoot(float time)
    {
        isShooting = true;
        Bullet bullet = Instantiate(Bullet, FirePos.transform.position, transform.rotation).GetComponent<Bullet>();
        bullet.speed = BulletSpeed;
        bullet.damage = Damage;
        yield return new WaitForSeconds(time);
        isShooting = false;
    }
}
