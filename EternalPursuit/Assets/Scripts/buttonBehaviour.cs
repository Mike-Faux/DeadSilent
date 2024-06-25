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
        StartCoroutine(wait());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.resumeState();
    }
    public void restartv2()
    {
        StartCoroutine(wait());

        SceneManager.LoadScene(2);
        GameManager.Instance.resumeState();
    }
    public void quit()
    {
        StartCoroutine(wait());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif

    }

    public void GoToNextLevel()
    {
        StartCoroutine(wait());
        GameManager.Instance.gameStats.Level++;
        GameManager.Instance.gameStats.TimeRemaining = GameManager.Instance.timeLimit.GetTime();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.resumeState();
    }

    public void Respawn()
    {
        GameManager.Instance.playerScript.SpawnPlayer(        GameManager.Instance.playerScript.GetTransform());
        GameManager.Instance.resumeState();
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
    }
}
