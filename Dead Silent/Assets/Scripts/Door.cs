using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    
    [SerializeField] float openRot, speed;
    public bool opening;

   

    void Update()
    {
       

    
    }
    void OpenRot()
    {

        Quaternion targetRot = Quaternion.Euler(0f, 40f, 0f);

       transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,targetRot, speed * Time.deltaTime);
    }

    public void Interact()
    {
        Debug.Log("interact");
        OpenRot();
        Debug.Log("OpenRot");
    }
}
