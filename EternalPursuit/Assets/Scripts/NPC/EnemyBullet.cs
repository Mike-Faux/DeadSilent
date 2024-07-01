using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f; 
    public int damage = 1;
    public float maxRange = 1000f;
    public ParticleSystem hitEffect;
    

    private Vector3 startPosition;
    private float travelDistance;
    [SerializeField] private float time = 3;

    public AudioSource impactSound;
    private Rigidbody rb;

    private Transform playerTransform;
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the bullet prefab", this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, time);

       rb.isKinematic = true; 
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Enemy bullets need a player to target.");
        }
    }
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized; 
    }
    void Update()
    {
        
        transform.position += direction * speed * Time.deltaTime;

        
        if (Vector3.Distance(startPosition, transform.position) >= maxRange)
        {
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
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with has the Player tag
        
        if (other.CompareTag("Player"))
        {
            
            ParticleSystem p = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(p.gameObject, p.main.duration + p.main.startLifetime.constant);

            AudioSource audioSource = Instantiate(impactSound, transform.position, Quaternion.identity);
            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length);

           
            if (other.gameObject.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(damage);
            }

            Destroy(gameObject); 
        }
    }
}

