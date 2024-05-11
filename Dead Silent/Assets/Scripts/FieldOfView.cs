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

    public List<GameObject> targets;


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
            if (Mathf.Abs(Vector3.Angle(transform.position, potentialTargets[i].transform.position)) <= ViewAngle)
            {
                Physics.Linecast(transform.position, potentialTargets[i].transform.position, obstructionMask);
                //Debug.Log($"{potentialTargets[i].name} found!");
                if (!targets.Contains(potentialTargets[i].gameObject)) {
                    targets.Add(potentialTargets[i].gameObject);
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
