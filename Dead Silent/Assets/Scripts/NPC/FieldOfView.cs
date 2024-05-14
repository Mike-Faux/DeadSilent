using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius;

    [Range(0, 360)]
    public float ViewAngle;
    bool TargetInRange;

    public LayerMask tarketMask;
    public LayerMask obstructionMask;

    [SerializeField] EnemyAI ai;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetInRange) CheckForTargets();
    }

    public Vector3 DirFromAngle(float angle, bool global)
    {
        if(!global)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public void CheckForTargets()
    {
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, ViewRadius, tarketMask);

        for(int i = 0; i < potentialTargets.Length; i++)
        {
            Vector3 targetDir = (potentialTargets[i].transform.position - transform.position).normalized;


            //Debug.Log($"{potentialTargets[i].name} found!");
            //Debug.Log(Physics.Linecast(transform.position, potentialTargets[i].transform.position, out RaycastHit hit, obstructionMask, QueryTriggerInteraction.Ignore));
            //Debug.Log(hit.collider.transform.name);

            if (Vector3.Angle(transform.forward, targetDir) <= ViewAngle / 2)
            {
                //Debug.Log($"{potentialTargets[i].name} is in Front!");
                if (!Physics.Linecast(transform.position, potentialTargets[i].transform.position, out RaycastHit hit, obstructionMask, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log(Vector3.Angle(transform.forward, targetDir));
                    //Debug.Log($"Tracking {potentialTargets[i].name}!");
                    ai.OnEnemySighted(potentialTargets[i].gameObject);

                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player Entered!");
            TargetInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player Left!");
            TargetInRange = false;
        }
    }

}
