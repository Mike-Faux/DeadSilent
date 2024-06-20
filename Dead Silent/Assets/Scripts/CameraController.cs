using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public int Sensitivity;
    [SerializeField] int LockVertMin, LockVertMax;
    [SerializeField] bool InvertY;

    float RotX;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;

        if (InvertY)
            RotX += mouseY;
        else
            RotX -= mouseY;

        RotX = Mathf.Clamp(RotX, LockVertMin, LockVertMax);

        transform.localRotation = Quaternion.Euler(RotX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
