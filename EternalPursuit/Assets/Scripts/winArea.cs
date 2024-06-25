using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.pauseState();
            GameManager.Instance.activeMenu = GameManager.Instance.winMenu;
            GameManager.Instance.activeMenu.SetActive(GameManager.Instance.pause);
        }
    }
}

