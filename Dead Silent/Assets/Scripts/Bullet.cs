using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public float speed = 1;
    public int damage = 1;

    [SerializeField] float time = 3;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, time);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamageable dmg))
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
