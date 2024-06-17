using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
[SerializedField] private float volume = 1f;
[SerializedField] private float sensitivity = 1f;
[SerializedField] private float quality = 1f;
[SerializedField] private float resolution = 1f;
[SerializedField] private int fullscreen;
[SerializedField] private int texture;
[SerializedField] private bool antialiasing;
[SerializedField] private bool vSync;

{
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


    public void UpdateFullscreen(bool isFullscreen)
    {
        screen.fullscreen = isFullscreen;
        Debug.Log(fullscreen);
    }

    public void UpdateVolume(float newVolume)
    {
        volume = newVolume;
        Debug.Log(volume);
    }
}
