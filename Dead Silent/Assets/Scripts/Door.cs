using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    
    [SerializeField] float openRot, speed;
    public bool opening;

   

    void Update()
    {
       

    
    }
    void OpenRot()
    {
        Quaternion targetRot = Quaternion.Euler(0f, 90f, 0f);

       transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,targetRot, speed * Time.deltaTime);
    }

    public void Interact()
    {
        //Debug.Log("interact");
        OpenRot();
    }
}
