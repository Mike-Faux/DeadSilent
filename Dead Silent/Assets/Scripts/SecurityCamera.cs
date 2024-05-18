using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] float rotateTime;
    [SerializeField] float pauseTime;
    [SerializeField] float rotationAngle;

    [SerializeField] Transform cam;

    readonly int segments = 80;
    float rotationTarget;
    bool isRotating;
    bool isPaused;



    // Start is called before the first frame update
    void Start()
    {
        rotationTarget = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRotating && !isPaused)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        float currRot = cam.localRotation.eulerAngles.y;

        if (currRot != 0)
        {
            StartCoroutine(SmoothRotate(0f));
            return;
        }

        StartCoroutine(SmoothRotate(rotationAngle * rotationTarget));
        rotationTarget *= -1f;
    }

    IEnumerator SmoothRotate(float angle)
    {
        isRotating = true;

        float interval = rotateTime / segments;

        float localRot = cam.localRotation.eulerAngles.y;
        if (localRot > 90) localRot -= 360;
        if (localRot < -90) localRot += 360;

        float rotation = (angle - localRot) / segments;

        //Debug.Log($"{rotation} {angle} {cam.localRotation.eulerAngles.y} {angle - (cam.localRotation.eulerAngles.y)}");

        for (int i = 0; i < segments; i++)
        {
            cam.Rotate(new Vector3 (0, rotation, 0), Space.Self);
            yield return new WaitForSeconds(interval);
        }

        cam.localRotation = Quaternion.Euler(0, angle, 0);

        if (angle != 0) StartCoroutine(Watch(pauseTime));
        isRotating = false;

    }

    IEnumerator Watch(float seconds)
    {
        isPaused = true;
        yield return new WaitForSeconds(seconds);
        isPaused = false;
    }
}
