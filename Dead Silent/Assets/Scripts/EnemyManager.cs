using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Material Patrol_Status_Material;
    [SerializeField] Material Idle_Status_Material;
    [SerializeField] Material Investigate_Status_Material;
    [SerializeField] Material Track_Status_Material;
    [SerializeField] Material Engage_Status_Material;

    public Material DamagedFlashMaterial;

    List<EnemyAI> Units;
    List<SecurityCamera> Cameras;

    [SerializeField] int EnemySearchCount = 3;

    private void Awake()
    {
        Units = new List<EnemyAI>();
        Cameras = new List<SecurityCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SC_ReportIn(SecurityCamera camera)
    {
        Cameras.Add(camera);
    }

    public void SC_ReportSighting(SecurityCamera sc, GameObject player)
    {
        OnSighting(player);
    }

    public void SC_ReportDestruction(SecurityCamera sc) 
    { 
        
    }

    public void ReportIn(EnemyAI enemyAI)
    {
        Units.Add(enemyAI);
    }

    public void SignOut(EnemyAI enemyAI)
    {
        Units.Remove(enemyAI);
    }

    public void OnSighting(GameObject player)
    {
        GameManager.Instance.LastKnownPosition = player.transform.position;
        foreach(EnemyAI enemyAI in GetClosestEnemies(player.transform.position, EnemySearchCount))
        {
            enemyAI.Alert();
        }
    }

    public List<EnemyAI> GetClosestEnemies(Vector3 pos, int count)
    {
        List<EnemyAI> closest = new List<EnemyAI>();
        List<EnemyAI> potentialTargets = Units;



        for(int i = 0; i < count; i++)
        {
            EnemyAI enemyAI = GetClosestEnemy(pos, potentialTargets);
            closest.Add(enemyAI);
            potentialTargets.Remove(enemyAI);
        }

        return closest;
    }

    public EnemyAI GetClosestEnemy(Vector3 pos, List<EnemyAI> potentialTargets)
    {
        EnemyAI target = null;
        float distance = Mathf.Infinity;

        foreach(EnemyAI enemyAI in potentialTargets)
        {
            float d = Vector3.Distance(enemyAI.transform.position, pos);
            if (d < distance)
            {
                target = enemyAI;
                distance = d;
            }
        }

        return target;
    }

    public Material GetStatusMaterial(EnemyAI.Status status)
    {
        switch (status)
        {
            default:                            return Patrol_Status_Material;
            case EnemyAI.Status.Engaging:       return Engage_Status_Material;
            case EnemyAI.Status.Investigating:  return Investigate_Status_Material;
            case EnemyAI.Status.Loitering:      return Idle_Status_Material;
            case EnemyAI.Status.Tracking:       return Track_Status_Material;
        }
    }}
