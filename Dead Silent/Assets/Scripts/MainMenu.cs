using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Transform settingsPanelTransform;
    public Camera mainCamera;
    public float rotationSpeed = 1.0f;

    [SerializeField] float sensitivity;
    [SerializeField] float quality;
    [SerializeField] float resolution;
    [SerializeField] bool fullscreen;
    [SerializeField] float volume;
    [SerializeField] Screen screen;
<<<<<<< Updated upstream
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

=======
<<<<<<< HEAD
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject mainMenuPanel;



    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    // Start is called before the first frame update
    public void Play()
=======
    public GameObject LoadScreen;
    public Image LoadingBar;
    public float delayBeforeLoading = 3f;


    public void LoadingScene(int sceneId)
>>>>>>> bdf170b483f49f69715c0ab5d0d5e67484ac92de
    {
        StartCoroutine(LoadSceneAsync(sceneId));
        Debug.Log("IsLoading");
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {

<<<<<<< HEAD
    public void ShowSettings()
    {
        settingsPanel.SetActive(true); // Show the settings canvas
        mainMenuPanel.SetActive(false); // Hide the main menu canvas
=======
>>>>>>> Stashed changes
       

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


>>>>>>> bdf170b483f49f69715c0ab5d0d5e67484ac92de
    }
    public void HideSettings()
    {
        settingsPanel.SetActive(false); // Hide the settings canvas
        mainMenuPanel.SetActive(true); // Show the main menu canvas
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
