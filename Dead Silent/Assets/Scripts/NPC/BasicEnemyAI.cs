using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class BasicEnemyAI : MonoBehaviour, IDamageable
{

    [SerializeField] NavMeshAgent agent;

    [SerializeField] int Health;
    [SerializeField] int EngageDistance;

    [SerializeField] float alertDistance;
    [SerializeField] float chaseDistance;
    [SerializeField] float attackDistance;
    [SerializeField] LayerMask toAlert;
    [SerializeField] LayerMask blockingFiring;
    [SerializeField] LayerMask Enemy;

    Vector3 targetPos;

    GameObject target;
    [SerializeField] IWeapon weapon;
    [SerializeField] GameObject weaponSlot;

    [SerializeField] GameObject StatusIndicator;
    GameObject player;
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateEnemyCount(1);
        player = GameManager.Instance.Player;
        mr = gameObject.GetComponent<MeshRenderer>();
        if (weaponSlot != null)
        {
            weapon = weaponSlot.GetComponentInChildren<IWeapon>();
            if (weapon == null)
            {
                Debug.LogError("Weapon is null!");
            }
        }

        weapon = weaponSlot.GetComponentInChildren<IWeapon>();
    }

    void Update()

    {

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= chaseDistance)
            {
                agent.SetDestination(player.transform.position);
                if (distanceToPlayer <= attackDistance)
                {
                    Debug.Log("Attacking Player");
                    Aim();
                    weapon.Attack();
                }
            }
        }
    }



    public void Awake()
    {
        attackDistance = 50f;
    }
    public void TakeDamage(int amount)
    {
        Health -= amount;
        StartCoroutine(Flash(.1f));

        if (Health <= 0)
        {
            OnDeath();
            return;
        }

        GameManager.Instance.LastKnownPosition = GameManager.Instance.Player.transform.position;
        target = GameManager.Instance.Player;
        targetPos = target.transform.position;
        agent.SetDestination(targetPos);
    }

    public void OnDeath()
    {
        agent.enabled = false;
        GameManager.Instance.UpdateEnemyCount(-1);
        Destroy(gameObject);
    }

    IEnumerator Flash(float time)
    {
        Material temp = mr.material;

        mr.material = GameManager.Instance.enemyManager.DamagedFlashMaterial;

        yield return new WaitForSeconds(time);

        mr.material = temp;
    }
    public void Aim()
    {
        Vector3 pos = player.transform.position;
        targetPos.y = transform.position.y;

    }
    public bool HasKey(KeySO key)
    {
        return false;
    }

    public enum Status
    {
        Patroling,
        Investigating,
        Tracking,
        Loitering,
        Engaging
    }
}
