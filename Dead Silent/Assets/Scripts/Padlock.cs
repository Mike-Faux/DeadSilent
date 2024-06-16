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
        
        
        
        GameManager.Instance.pauseState();
        GameManager.Instance.activeMenu = GameManager.Instance.winMenu;
        GameManager.Instance.activeMenu.SetActive(GameManager.Instance.pause);

        

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

         
            }
            

        }

    public void Interact(Player user)
    {
        Interact();
    }

    public void Interact(EnemyAI user)
    {
        return;
    }
}
