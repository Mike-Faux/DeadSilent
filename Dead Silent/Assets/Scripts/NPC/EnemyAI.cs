using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable, IDistractable
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] int Health;
    [SerializeField] int EngageDistance;
    [SerializeField] AIType type;

    [SerializeField] float alertDistance;
    [SerializeField] LayerMask toAlert;
    [SerializeField] LayerMask blockingFiring;

    (Vector3 Position, int TimeInPosition)[] patrolPath;
    [SerializeField] Status currentStatus;

    int currentPatrolPoint;
    Vector3 targetPos;

    GameObject target;
    [SerializeField] FireArm weapon;


    MeshRenderer mr;
    [SerializeField] Material DamagedFlashMaterial;

    float LoiterVariation = 1.5f;



    public void TakeDamage(int amount)
    {
        Health -= amount;
        StartCoroutine(Flash(.1f));

        if(Health <= 0) 
        { 
            GameManager.Instance.UpdateEnemyCount(-1);
            Destroy(gameObject);
        }

        GameManager.Instance.LastKnownPosition = GameManager.Instance.Player.transform.position;
        currentStatus = Status.Tracking;
        target = GameManager.Instance.Player;
        targetPos = target.transform.position;
        agent.SetDestination(targetPos);
    }

    IEnumerator Flash(float time)
    {
        Material temp = mr.material;

        mr.material = DamagedFlashMaterial;

        yield return new WaitForSeconds(time);

        mr.material = temp;
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateEnemyCount(1);
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStatus != Status.Engaging) agent.isStopped = false;
        if (currentStatus == Status.Engaging && agent.remainingDistance > 1f)
        {
            Engage();
        }

        if(Vector3.Distance(agent.destination, transform.position) < 2f)
        {
            switch(currentStatus)
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

    public void Engage()
    {
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

        float disToTarget = Vector3.Distance(target.transform.position, transform.position);
        Vector3 dirToTarget = target.transform.position - transform.position;
        dirToTarget.Normalize();


        //Debug.Log($"{name} Engaging target {disToTarget} away");

        //Check for engage Distance and target
        if (disToTarget > EngageDistance || target == null)
        {
            agent.isStopped = false;
            currentStatus = Status.Tracking;
        }
        else if (!Physics.Raycast(transform.position + transform.forward, dirToTarget, disToTarget, blockingFiring, QueryTriggerInteraction.Ignore))
        {
            currentStatus = Status.Engaging;
            agent.isStopped = true;
            if(weapon != null)
            {
                Aim();
                weapon.Attack();
            }
        }
        else
        {
            currentStatus = Status.Tracking;
            agent.SetDestination(target.transform.position);
            agent.isStopped = false;
        }
    }

    public void Aim()
    {
        transform.LookAt(target.transform.position - (transform.right / 2), Vector3.up);
    }

    public void Track()
    {
        if(target != null && Vector3.Distance(target.transform.position, transform.position) < EngageDistance)
        {
            currentStatus = Status.Engaging;
            Engage();
        }else if (targetPos != GameManager.Instance.LastKnownPosition)
        {
            targetPos = GameManager.Instance.LastKnownPosition;
            agent.SetDestination(targetPos);
        }
        else
        {
            target = null;
            currentStatus = Status.Investigating;
        }
    }

    public void Investigate()
    {
        //Debug.Log("Investigating");
        if(targetPos == GameManager.Instance.LastKnownPosition)
        {
            currentStatus = Status.Loitering;
            StartCoroutine(Loiter(10, currentPatrolPoint));
        }
        else
        {
            targetPos = GameManager.Instance.LastKnownPosition;
            agent.SetDestination(targetPos);
            currentStatus = Status.Tracking;
        }
    }

    public void Patrol()
    {
        if (patrolPath == null) return;
        if (patrolPath.Length == 0) return;
        int seconds = patrolPath[currentPatrolPoint].TimeInPosition;

        currentPatrolPoint++;

        if(currentPatrolPoint < 0 || currentPatrolPoint >= patrolPath.Length)
        {
            currentPatrolPoint = 0;

            if(patrolPath.Length == 0) return; 
        }

        currentStatus = Status.Loitering;
        StartCoroutine(Loiter(seconds, currentPatrolPoint));
    }

    IEnumerator Loiter(int seconds, int nextPatrolPoint)
    {
        float min = seconds - LoiterVariation;
        if(min < 0) min = 0;
        float max = seconds + LoiterVariation;

        float split = Random.Range(min, max) / 3;
        if(split < 1f) split = 1f;

        yield return new WaitForSeconds(split);
        if (currentStatus != Status.Loitering) yield break;

        StartCoroutine(SmoothRotate(Quaternion.LookRotation(-transform.right, transform.up),30));

        yield return new WaitForSeconds(split);
        if (currentStatus != Status.Loitering) yield break;

        StartCoroutine(SmoothRotate(Quaternion.LookRotation(-transform.forward, transform.up),30));

        yield return new WaitForSeconds(split);
        if (currentStatus != Status.Loitering) yield break;

        agent.SetDestination(patrolPath[nextPatrolPoint].Position);
        currentStatus = Status.Patroling;
    }

    IEnumerator SmoothRotate(Quaternion targetRotation,int segments)
    {
        float interval = 1f / segments;

        transform.Rotate(new Vector3(0, 0, 0));

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

        
        currentStatus = Status.Engaging;
        agent.SetDestination(target.transform.position);
        Engage();
    }

    public void Distract(GameObject distraction)
    {
        if (currentStatus == Status.Tracking) return;

        targetPos = distraction.transform.position;
        currentStatus = Status.Investigating;
        agent.SetDestination(targetPos);
    }

    public void Alert()
    {
        if (currentStatus == Status.Engaging) return;

        targetPos = GameManager.Instance.LastKnownPosition;
        agent.SetDestination(targetPos);
        currentStatus = Status.Tracking;
    }

    public void SetPatrolPath((Vector3 Position, int TimeInPosition)[] path)
    {
        //Debug.Log($"{name} path set with {path.Length} points!");
        patrolPath = path;
    }

    enum Status
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
