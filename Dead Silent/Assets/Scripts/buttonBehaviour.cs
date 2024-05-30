using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonBehaviour : MonoBehaviour
{
    public void resume()
    {
        GameManager.Instance.resumeState();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.resumeState();
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif

    }

    public void Respawn()
    {
        GameManager.Instance.playerScript.SpawnPlayer();
        GameManager.Instance.resumeState();
    }
}
