using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Renderer model;

    [SerializeField] Color cpFlashColor = Color.red;
    [SerializeField] Color cpOrigColor = Color.white;
    [SerializeField] float cpPopupTime = 1.5f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.playerSpawnPos.transform.position != transform.position)
        {
            GameManager.Instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(DisplayPopup());
        }
    }

    IEnumerator DisplayPopup()
    {
        model.material.color = cpFlashColor;
        GameManager.Instance.checkpointPopup.SetActive(true);
        yield return new WaitForSeconds(cpPopupTime);
        GameManager.Instance.checkpointPopup.SetActive(false);
        model.material.color = cpOrigColor;
    }
}
