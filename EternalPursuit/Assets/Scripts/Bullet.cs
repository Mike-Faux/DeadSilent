using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    public float speed = 1000f;
    public int damage = 1;
    public float maxRange = 1000f;
    public ParticleSystem hitEffect;

    private Vector3 startPosition;
    private float travelDistance;
    [SerializeField] float time = 3;

    public AudioSource impactSound;

    
    void Start()
    {
        startPosition = transform.position;
       
        Destroy(gameObject, time);
    }
    public void SetDirection(Vector3 direction)
    {
        rb.velocity = direction.normalized * speed;
    }
    public void InitializeWithTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.velocity = direction * speed; 
    }


    private void Update()
    {
        travelDistance = Vector3.Distance(startPosition, transform.position);

        if(travelDistance >= maxRange ) { 
        Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        ParticleSystem p = Instantiate(hitEffect, collision.contacts[0].point, Quaternion.identity);
        Destroy(p.gameObject, p.main.duration + p.main.startLifetime.constant);

        AudioSource audioSource = Instantiate(impactSound, collision.contacts[0].point, Quaternion.identity);
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
        
        
        if (collision.gameObject.TryGetComponent(out IDamageable dmg))

        
        {
            dmg.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
