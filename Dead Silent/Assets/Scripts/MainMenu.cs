using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] float sensitivity;
    [SerializeField] float quality;
    [SerializeField] float resolution;
    [SerializeField] bool fullscreen;
    [SerializeField] float volume;
    [SerializeField] Screen screen;



    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
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
