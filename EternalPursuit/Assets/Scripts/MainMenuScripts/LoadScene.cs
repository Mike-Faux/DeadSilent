using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LoadScreen;
   
    public Slider slider;

    public void StartLoadingScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    public IEnumerator LoadSceneAsync(int sceneId)
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        LoadScreen.SetActive(true);
        

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;    
        
        yield return null;
    }
        LoadScreen.SetActive(false);
    }
    
}




