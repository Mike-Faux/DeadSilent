using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, Weapon
{

    [SerializeField] Transform SwingPos;
    [SerializeField] int Damage;
    [SerializeField] float SwingRate;
    [SerializeField] float SwingRadius;


    bool isSwinging;



    public void Attack()
    {
        if (!isSwinging)
        {
            StartCoroutine(Swing(SwingRate));
        }
    }

    IEnumerator Swing(float time)
    {
        isSwinging = true;
        Collider[] colliders = Physics.OverlapSphere(SwingPos.position, SwingRadius);
        //Debug.Log(colliders.Length);

        for(int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log(colliders[i].name);
            if (colliders[i].TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(Damage);
            }
        }


        yield return new WaitForSeconds(time);
        isSwinging = false;
    }
}
