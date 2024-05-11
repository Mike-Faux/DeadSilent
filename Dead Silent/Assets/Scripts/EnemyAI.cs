using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] int Health;

    FieldOfView fov;


    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
