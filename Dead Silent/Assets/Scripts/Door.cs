using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door;
    [SerializeField] float openRot, speed;
    [SerializeField] GameObject intIcon;
    public bool opening;

    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay called");
        if (other.CompareTag("MainCamera"))
        {
            intIcon.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed");
                intIcon.SetActive(false);
                opening = true;
                OpenRot();
                
            }
        }
    }
    void Update ()
    {
        OpenRot();
    }
    void OpenRot()
    {
        Vector3 currentRot = door.transform.localEulerAngles;
        if (opening)
        {

            if (currentRot.y < openRot)
            {
                door.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openRot, currentRot.z), speed * Time.deltaTime);
            }

        }
    }
}
