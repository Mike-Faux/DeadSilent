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

    [SerializeField] float RereportDelay = 10;

    readonly int segments = 80;
    float rotationTarget;
    bool isRotating;
    bool isPaused;

    bool isDestroyed = false;
    bool isWatched;
    bool isReported;

    //public void OnPlayerDetected(GameObject player)
    //{
    //    if(isWatched && !isReported)
    //    {
    //        GameManager.Instance.enemyManager.SC_ReportSighting(this, player);
    //        StartCoroutine(Report(RereportDelay));
    //    }
    //}

    public void OnDestruction()
    {
        Debug.Log("Security Camera Destroyed!");
        GameManager.Instance.enemyManager.SC_ReportDestruction(this);
        cam.gameObject.SetActive(false);
        isDestroyed = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        isWatched = true;

        rotationTarget = 1f;
        GameManager.Instance.enemyManager.SC_ReportIn(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDestroyed) return;

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

    IEnumerator Report(float time)
    {
        isReported = true;
        yield return new WaitForSeconds(time);
        isReported = false;
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
