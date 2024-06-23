using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 100f;
    [SerializeField] int LockVertMin, LockVertMax;
    [SerializeField] bool InvertY;

    float RotX;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }


    // Update is called once per frame
    public void Update()
    {
        // Mouse input
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // Joystick input (add these lines)
        float joystickY = Input.GetAxis("RightStickVertical") * sensitivity * Time.deltaTime;
        float joystickX = Input.GetAxis("RightStickHorizontal") * sensitivity * Time.deltaTime;

        // Combine mouse and joystick input
        float combinedY = mouseY + joystickY;
        float combinedX = mouseX + joystickX;

        if (InvertY)
            RotX += combinedY;
        else
            RotX -= combinedY;

        RotX = Mathf.Clamp(RotX, LockVertMin, LockVertMax);

        transform.localRotation = Quaternion.Euler(RotX, 0, 0);

        transform.parent.Rotate(Vector3.up * combinedX);
    }
}
