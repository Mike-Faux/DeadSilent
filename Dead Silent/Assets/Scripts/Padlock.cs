using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Padlock : MonoBehaviour, IInteractable
{
    
    [SerializeField] string previousScene;

    public void StartMinigame()
    {

        previousScene = SceneManager.GetActiveScene().name;
       
        SceneManager.LoadScene("Minigame");
    }

    public void CompleteMinigame()
    {
        
        SceneManager.LoadScene("Level_M2");
       
    }


    public void Interact()
    {
        Debug.Log("Minigame");
        StartMinigame();
        
    }

    void OnCollisionEnter(Collision collision)
        {
           
            if (collision.gameObject.CompareTag("Player"))
            {

            CompleteMinigame();

            Destroy(gameObject);
        }
        }
        
    }
