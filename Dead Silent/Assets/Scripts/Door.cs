using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    
    [SerializeField] float openRot, speed;
    bool opening;
    bool delay;

    Quaternion targetRotation;


    private void Start()
    {
        opening = false;
        targetRotation = transform.rotation;
    }


    void Update()
    {
        transform.parent.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);

    
    }

    void OpenRot()
    {

        Quaternion targetRot = Quaternion.Euler(0f, 40f, 0f);

        transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,targetRot, speed * Time.deltaTime);
    }

    public void Interact()
    {
        if (delay) return;

        //Debug.Log("interact");
        StartCoroutine(Delay());
        if (opening)
        {
            targetRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        opening = !opening;
        //Debug.Log("OpenRot");
    }

    IEnumerator Delay()
    {
        delay = true;
        yield return new WaitForSeconds(.1f);
        delay = false;
    }
}
