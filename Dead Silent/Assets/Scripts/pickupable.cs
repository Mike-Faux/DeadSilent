using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string itemName;

    [SerializeField]
     private int ItemAmount;

    [SerializeField]
    private string itemDescription;




    private GameManager gameManager;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }


    // Called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the Player tag or any other relevant tag
        if (other.CompareTag("Player"))
        {
            // Trigger the pickup action
           int leftOverItems = gameManager.AddItem(itemName, ItemAmount, itemDescription);
           if(leftOverItems <= 0)
            {
                gameManager.IncrementItemCount(ItemAmount);
                Destroy(gameObject);
            }
            else
            {
                gameManager.IncrementItemCount(ItemAmount - leftOverItems);
                ItemAmount = leftOverItems;
            }
        }
    }

    // Pickup action logic
    public void Interact()
    {
       
    }
}