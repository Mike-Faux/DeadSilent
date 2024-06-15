using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class SecurityCamDetection : MonoBehaviour, IDamageable
//{
//    [SerializeField] SecurityCamera securityCamera;
//    [SerializeField] Transform camPos;

//    public LayerMask obstructionMask;


//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            if(!Physics.Linecast(camPos.position, other.transform.position, obstructionMask, QueryTriggerInteraction.Ignore))
//            {
//                securityCamera.OnPlayerDetected(other.gameObject);
//            }
//        }
//    }

//    public void TakeDamage(int amount)
//    {
//        securityCamera.OnDestruction();
//    }
//}
