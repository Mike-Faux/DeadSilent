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

    private void Awake()
    {
        Units = new List<EnemyAI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    }
}
