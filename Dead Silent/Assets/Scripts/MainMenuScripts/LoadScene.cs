using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LoadScreen;
    public Image LoadingBar;


   
   public IEnumerator LoadSceneAsync(int sceneId)
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        LoadScreen.SetActive(true);

        

        while (!operation.isDone)
        {
           

            yield return null;
        }
    }
}



