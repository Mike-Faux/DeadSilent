using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] float pauseTime;
    [SerializeField] float rotationAngle;

    [SerializeField] Transform cam;

    int segments = 30;
    int rotationTarget;
    bool isRotating;
    bool isPaused;



    // Start is called before the first frame update
    void Start()
    {
        rotationTarget = 1;
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
        StartCoroutine(SmoothRotate(Quaternion.AngleAxis(rotationAngle * rotationTarget, Vector3.forward)));
        rotationTarget *= -1;
    }

    IEnumerator SmoothRotate(Quaternion targetRotation)
    {
        isRotating = true;
        float interval = rotateSpeed / segments;

        //cam.Rotate(new Vector3(0, 0, 0));

        for (int i = 0; i < segments; i++)
        {
            cam.rotation = Quaternion.Slerp(cam.rotation, targetRotation, i * interval + interval);
            yield return new WaitForSeconds(interval);
        }

        isRotating = false;

        StartCoroutine(Watch(pauseTime));
    }

    IEnumerator Watch(float seconds)
    {
        isPaused = true;
        yield return new WaitForSeconds(seconds);
        isPaused = false;
    }
}
