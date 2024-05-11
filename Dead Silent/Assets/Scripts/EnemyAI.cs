using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int Health;

    public PatrolWaypoint[] PatrolPath;
    [SerializeField] Status currentStatus;

    int currentPatrolPoint;
    Vector3 target;



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
            }
        }
    }


    public void Track()
    {
        //Debug.Log("Tracking");
        if (target != GameManager.Instance.LastKnownPosition)
        {
            target = GameManager.Instance.LastKnownPosition;
            agent.destination = target;
        }
        else
        {
            currentStatus = Status.Investigating;
        }
    }

    public void Investigate()
    {
        //Debug.Log("Investigating");
        if(target == GameManager.Instance.LastKnownPosition)
        {
            currentStatus = Status.Loitering;
            StartCoroutine(Loiter(10, currentPatrolPoint));
        }
        else
        {
            target = GameManager.Instance.LastKnownPosition;
            agent.destination = target;
            currentStatus = Status.Tracking;
        }
    }

    public void Patrol()
    {
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
        currentStatus = Status.Tracking;
        GameManager.Instance.LastKnownPosition = target.transform.position;
        agent.destination = target.transform.position;
        this.target = target.transform.position;
    }

    enum Status
    {
        Patroling,
        Investigating,
        Tracking,
        Loitering
    }
}
