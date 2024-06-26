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
    [SerializeField] public  float delayBeforeLoading = 1f;
    public Slider slider;

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject controlPanel;
    [SerializeField] GameObject creditsPanel;

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        controlPanel.SetActive(false);
    }

    public void LoadingScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
        //Debug.Log("IsLoading");
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {

       

        LoadScreen.SetActive(true);

        yield return new WaitForSeconds(delayBeforeLoading);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }

    }
    public void showControls()
    {
        controlPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
    public void hideControls()
    {
        controlPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    public void ShowSettings()
    {
        settingsPanel.SetActive(true);  // Show the settings canvas
        mainMenuPanel.SetActive(false);
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false); // Hide the settings canvas
        mainMenuPanel.SetActive(true);  // Show the main menu canvas
    }

    public void showCredits()
    {
        creditsPanel.SetActive(true);   // Show credits
        mainMenuPanel.SetActive(false); // Hide menu
    }
    
    public void hideCredits()
    {
        creditsPanel.SetActive(false);  // Hide credits
        mainMenuPanel.SetActive(true);  // Show main menu
    }

    public void Quit()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

  

  

  




  

}
