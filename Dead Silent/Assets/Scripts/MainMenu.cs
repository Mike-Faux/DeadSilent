using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] float sensitivity;
    [SerializeField] float quality;
    [SerializeField] float resolution;
    [SerializeField] bool fullscreen;
    [SerializeField] float volume;
    [SerializeField] Screen screen;
    public GameObject LoadScreen;
    public Image LoadingBar;
    public float delayBeforeLoading = 3f;


    public void LoadingScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
        Debug.Log("IsLoading");
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {

       

        LoadScreen.SetActive(true);

        yield return new WaitForSeconds(delayBeforeLoading);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {


            yield return null;
        }
    }
    public void Settings()
    { 


    }
    public void Quit()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void UpdateSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        Debug.Log(sensitivity);
    }

    public void UpdateQuality(float newQuality)
    {
        quality = newQuality;
        Debug.Log(quality);
    }

    public void UpdateResolution(float newResolution)
    {
        resolution = newResolution;
        Debug.Log(resolution);
    }




    public void UpdateVolume(float newVolume)
    {
        volume = newVolume;
        Debug.Log(volume);
    }

}
