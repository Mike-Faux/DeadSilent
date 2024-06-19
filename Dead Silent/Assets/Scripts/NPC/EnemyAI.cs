using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyAI : MonoBehaviour, IDamageable
{

    [SerializeField] NavMeshAgent agent;

    [SerializeField] int Health;
    [SerializeField] int EngageDistance;
    [SerializeField] AIType type;

    [SerializeField] float alertDistance;
    [SerializeField] float chaseDistance;
    [SerializeField] float attackDistance;
    [SerializeField] LayerMask toAlert;
    [SerializeField] LayerMask blockingFiring;
    [SerializeField] LayerMask Enemy;
    (Vector3 Position, int TimeInPosition)[] patrolPath;
    [SerializeField] Status currentStatus;

    float BaseSpeed;
    Animator animator;


    int currentPatrolPoint;
    Vector3 targetPos;

    GameObject target;
    [SerializeField] IWeapon weapon;
    [SerializeField] GameObject weaponSlot;

    [SerializeField] GameObject StatusIndicator;
    MeshRenderer StatusIndicatorMR;
    MeshRenderer mr;

    float LoiterVariation = 1.5f;
    float runSpeedThreshold = 5f;
    //float maxRange = 100f;
    readonly int isRunningHash = Animator.StringToHash("isRunning");

    public bool AlwaysAware;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.enemyManager.ReportIn(this);
        GameManager.Instance.UpdateEnemyCount(1);

        mr = gameObject.GetComponent<MeshRenderer>();
        if (weaponSlot != null)
        {
            weapon = weaponSlot.GetComponentInChildren<IWeapon>();
            if (weapon == null)
            {
                Debug.LogError("Weapon is null!");
            }
        }
        if (!StatusIndicator.TryGetComponent(out StatusIndicatorMR))
        {
            Debug.Log("Not Found");
        }

        BaseSpeed = agent.speed;

        weapon = weaponSlot.GetComponentInChildren<IWeapon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //AlwaysAware Condition
        if (AlwaysAware)
        {
            if (Vector3.Distance(GameManager.Instance.LastKnownPosition, GameManager.Instance.Player.transform.position) > 5)
                GameManager.Instance.LastKnownPosition = GameManager.Instance.Player.transform.position;
            
            if (currentStatus == Status.Loitering || currentStatus == Status.Patroling) 
                currentStatus = Status.Tracking;
        }

        //Animation
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // If the agent is not moving, play the "idle" animation
            animator.SetBool("isWalking", false);
            animator.SetBool(isRunningHash, false);
        }
        else if (agent.velocity.magnitude > runSpeedThreshold)
        {
            // If the agent's speed is above the threshold, set "isRunning" to true
            animator.SetBool(isRunningHash, true);
        }
        else
        {
            // If the agent is moving, play the "walk" animation
            animator.SetBool("isWalking", true);
        }

        //If target is close enough, Engage
        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= EngageDistance)
        {

            Engage();
        }


        //Main Switch Statement
        if (Vector3.Distance(agent.destination, transform.position) < 2f)
        {
            switch (currentStatus)
            {
                default:
                    Patrol();
                    break;
                case Status.Investigating:
                    Investigate();
                    break;
                case Status.Tracking:
                    Track();
                    break;
                case Status.Loitering:
                    break;
                case Status.Engaging:
                    Engage();
                    break;
            }
        }
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
        SetStatus(Status.Tracking);
        target = GameManager.Instance.Player;
        targetPos = target.transform.position;
        agent.SetDestination(targetPos);
    }

    public void OnDeath()
    {
        agent.enabled = false;
        GameManager.Instance.enemyManager.SignOut(this);
        GameManager.Instance.UpdateEnemyCount(-1);
        GameManager.Instance.gameStats.Kills++;
        Destroy(gameObject);
    }

    IEnumerator Flash(float time)
    {
        Material temp = mr.material;

        mr.material = GameManager.Instance.enemyManager.DamagedFlashMaterial;

        yield return new WaitForSeconds(time);
        
        mr.material = temp;
    }

    public void Engage()
    {
        agent.speed = BaseSpeed * 1.5f;
        //Officer Alerting Nearby Units
        if (type == AIType.Officer && GameManager.Instance.LastKnownPosition != null)
        {
            Collider[] nearby = Physics.OverlapSphere(transform.position, alertDistance, toAlert);
            for (int i = 0; i < nearby.Length; i++)
            {
                if (nearby[i].TryGetComponent(out EnemyAI ai))
                {
                    ai.Alert();
                    //Debug.Log("ALERT!");
                }
            }
        }

        //If no target, Return
        if(target == null)
        {
            Debug.Log("TargetNull");
            return;
        }

        float disToTarget = Vector3.Distance(target.transform.position, 
            transform.position);
        Vector3 dirToTarget = target.transform.position - transform.position;
        dirToTarget.Normalize();


        //Debug.Log($"{name} Engaging target {disToTarget} away");

        //Check for engage Distance and target
        if (disToTarget > EngageDistance || target == null)
        {
            agent.isStopped = false;

            SetStatus(Status.Tracking);
        }
        else if (!Physics.Raycast(transform.position + transform.forward, dirToTarget, disToTarget, blockingFiring, QueryTriggerInteraction.Ignore))
        {
            SetStatus(Status.Engaging);
            agent.isStopped = true;
            if (weapon != null)
            {
                if (weapon.GetType() == typeof(FireArm))
                {
                    FireArm gun = (FireArm)weapon;
                    if (gun.Ammo < 1) gun.Reload();
                }

                Aim();
                weapon.Attack();
            }
        }
        else
        {
            SetStatus(Status.Tracking);
            agent.SetDestination(target.transform.position);
            agent.isStopped = false;
        }
    }

    public void Aim()
    {
        Vector3 pos = target.transform.position;
        pos.y = transform.position.y;
        transform.LookAt(pos - (transform.right / 2), Vector3.up);
    }

    public void Track()
    {
        agent.speed = BaseSpeed * 1.5f;
        if (target != null && Vector3.Distance(target.transform.position, transform.position) < EngageDistance)
        {
            SetStatus(Status.Engaging);
            Engage();
        }
        else if (targetPos != GameManager.Instance.LastKnownPosition)
        {
            targetPos = GameManager.Instance.LastKnownPosition;
            agent.SetDestination(targetPos);
        }
        else
        {
            target = null;
            SetStatus(Status.Investigating);
        }
    }

    public void Investigate()
    {
        agent.speed = BaseSpeed * .8f;
        //Debug.Log("Investigating");
        if (targetPos == GameManager.Instance.LastKnownPosition)
        {
            SetStatus(Status.Loitering);
            StartCoroutine(Loiter(10, currentPatrolPoint));
        }
        else
        {
            targetPos = GameManager.Instance.LastKnownPosition;
            agent.SetDestination(targetPos);
            SetStatus(Status.Tracking);
        }
    }

    public void Patrol()
    {
        agent.speed = BaseSpeed;
        if (patrolPath == null) return;
        if (patrolPath.Length == 0) return;
        int seconds = patrolPath[currentPatrolPoint].TimeInPosition;

        currentPatrolPoint++;

        if (currentPatrolPoint < 0 || currentPatrolPoint >= patrolPath.Length)
        {
            currentPatrolPoint = 0;

            if (patrolPath.Length == 0) return;
        }

        SetStatus(Status.Loitering);
        StartCoroutine(Loiter(seconds, currentPatrolPoint));
    }

    IEnumerator Loiter(int seconds, int nextPatrolPoint)
    {
        float min = seconds - LoiterVariation;
        if (min < 0) min = 0;
        float max = seconds + LoiterVariation;

        float split = Random.Range(min, max) / 3;
        if (split < 1f) split = 1f;

        yield return new WaitForSeconds(split);
        if (currentStatus != Status.Loitering) yield break;

        StartCoroutine(SmoothRotate(Quaternion.LookRotation(-transform.right, transform.up), 30));

        yield return new WaitForSeconds(split);
        if (currentStatus != Status.Loitering) yield break;

        StartCoroutine(SmoothRotate(Quaternion.LookRotation(-transform.forward, transform.up), 30));

        yield return new WaitForSeconds(split);
        if (currentStatus != Status.Loitering) yield break;

        agent.SetDestination(patrolPath[nextPatrolPoint].Position);
        SetStatus(Status.Patroling);
    }

    IEnumerator SmoothRotate(Quaternion targetRotation, int segments)
    {
        float interval = 1f / segments;

        for (int i = 0; i < segments; i++)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, i * interval + interval);
            yield return new WaitForSeconds(interval);
            if (currentStatus != Status.Loitering) break;
        }
    }

    public void OnEnemySighted(GameObject target)
    {
        this.target = target;
        GameManager.Instance.LastKnownPosition = target.transform.position;
        this.targetPos = target.transform.position;


        SetStatus(Status.Tracking);
        agent.SetDestination(target.transform.position);
        Engage();
    }

    public void Distract(GameObject distraction)
    {
        if (currentStatus == Status.Tracking) return;

        targetPos = distraction.transform.position;
        SetStatus(Status.Investigating);
        agent.SetDestination(targetPos);
    }

    public void Alert()
    {
        if (currentStatus == Status.Engaging) return;

        targetPos = GameManager.Instance.LastKnownPosition;
        agent.SetDestination(targetPos);
        SetStatus(Status.Tracking);
    }

    public void SetPatrolPath((Vector3 Position, int TimeInPosition)[] path)
    {
        //Debug.Log($"{name} path set with {path.Length} points!");
        patrolPath = path;
    }

    public void SetStatus(Status status)
    {
        currentStatus = status;
        if (StatusIndicatorMR == null)
        {
            Debug.Log("What?");
            StatusIndicator.TryGetComponent(out StatusIndicatorMR);
        }
        StatusIndicatorMR.material = GameManager.Instance.enemyManager.GetStatusMaterial(status);
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

    enum AIType
    {
        Grunt,
        Officer
    }

}
