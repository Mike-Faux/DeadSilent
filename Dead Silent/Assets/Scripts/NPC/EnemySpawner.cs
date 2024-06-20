using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Spawning;

    public int spawnCount;
    public int spawnDelay;

    public bool UsePatrolPoints;
    public List<PatrolWaypoint> PatrolPath;
    public bool SpawnOnFirstPoint;

    GameObject spawn;
    bool spawning;
    bool delay;

    (Vector3 Position, int TimeInPosition)[] path;
    EnemyAI ai;

    // Start is called before the first frame update
    void Start()
    {
        spawning = false;
        delay = false;

        path = new (Vector3 Position, int TimeInPosition)[PatrolPath.Count];
        ai = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning && !delay)
        {
            StartCoroutine(SpawnDelay());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawning = true;
        }
    }

    IEnumerator SpawnDelay()
    {
        Spawn();
        delay = true;
        yield return new WaitForSeconds(spawnDelay);
        delay = false;
    }

    void Spawn()
    {

        UsePatrolPoints = false;
        if (UsePatrolPoints)
        {
            if (SpawnOnFirstPoint && PatrolPath.Count > 0)
            {
                spawn = Instantiate(Spawning, PatrolPath[0].transform.position, transform.rotation);
            }
            else
            {
                spawn = Instantiate(Spawning, transform.position, transform.rotation);
            }

            ai = spawn.GetComponent<EnemyAI>();


            for (int i = 0; i < PatrolPath.Count; i++)
            {
                path[i] = new(PatrolPath[i].transform.position, PatrolPath[i].TimeInPosition);
            }

            ai.SetPatrolPath(path);
        }

        spawn = Instantiate(Spawning, transform.position, transform.rotation);
        ai = spawn.GetComponent<EnemyAI>();
        Destroy(gameObject);
    }
}
