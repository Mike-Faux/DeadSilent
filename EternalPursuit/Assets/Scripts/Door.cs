using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    
    [SerializeField] float openRot, speed;

    public bool locked = false;
    [SerializeField] KeySO key;

    bool opening;
    bool delay;

    Quaternion targetRotation;


    private void Start()
    {
        opening = false;
        targetRotation = transform.rotation;
    }


    void FixedUpdate()
    {
        transform.parent.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }
    

    void ToggleDoor()
    {
        StartCoroutine(Delay());
        if (opening)
        {
            Open();
        }
        else
        {
            Close();
        }

        opening = !opening;
    }
    public void Open()
    {
        targetRotation = Quaternion.Euler(0f, 90f, 0f);
    }

    public void Close()
    {
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    IEnumerator Delay()
    {
        delay = true;
        yield return new WaitForSeconds(.1f);
        delay = false;
    }
    public void Interact()
    {
        if (!opening && locked) return;
        if (delay) return;
        ToggleDoor();
    }

    public void Interact(Player user)
    {
        if (delay) return;
        if (locked && !user.HasKey(key)) return;
        ToggleDoor();
    }

    public void Interact(EnemyAI user)
    {
        if (delay) return;
        if (locked && !user.HasKey(key)) return;
        ToggleDoor();
    }
}
