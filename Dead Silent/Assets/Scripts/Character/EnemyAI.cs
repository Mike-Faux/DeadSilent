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

    public PatrolWaypoint[] PatrolPath;
    [SerializeField] Status currentStatus;

    int currentPatrolPoint;
    Vector3 targetPos;

    GameObject target;
    [SerializeField] FireArm weapon;



    public void TakeDamage(int amount)
    {
        Health -= amount;

        if(Health <= 0) 
        { 
            GameManager.Instance.UpdateEnemyCount(-1);

        }



        currentStatus = Status.Tracking;
        Track();
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UpdateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentStatus != Status.Engaging) agent.isStopped = false;

        if(agent.remainingDistance <= 1f)
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
        if(Vector3.Distance(target.transform.position, transform.position) > EngageDistance || target == null)
        {
            currentStatus = Status.Tracking;
        }
        else if (Physics.Raycast(transform.position, (target.transform.position - transform.position).normalized))
        {
            currentStatus = Status.Engaging;
            agent.isStopped = true;
            if(weapon != null)
            {
                Aim();
                weapon.Attack();
            }
        }
    }

    public void Aim()
    {
        transform.LookAt(target.transform.position, Vector3.up);
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
            agent.destination = targetPos;
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
            agent.destination = targetPos;
            currentStatus = Status.Tracking;
        }
    }

    public void Patrol()
    {
        if (PatrolPath.Length == 0) return;
        int seconds = PatrolPath[currentPatrolPoint].TimeInPosition;

        currentPatrolPoint++;

        if(currentPatrolPoint < 0 || currentPatrolPoint >= PatrolPath.Length)
        {
            currentPatrolPoint = 0;

            if(PatrolPath.Length == 0) return; 
        }

        currentStatus = Status.Loitering;
        StartCoroutine(Loiter(seconds, currentPatrolPoint));
    }

    IEnumerator Loiter(int seconds, int nextPatrolPoint)
    {
        yield return new WaitForSeconds(seconds);
        if (currentStatus != Status.Loitering) yield break;

        agent.SetDestination(PatrolPath[nextPatrolPoint].transform.position);
        currentStatus = Status.Patroling;
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
