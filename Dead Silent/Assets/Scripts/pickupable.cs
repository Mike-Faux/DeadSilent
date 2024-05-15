using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, IInteractable
{
    public string itemName;
    private GameManager gameManager;


    private void Start()
    {
        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
    }


    // Called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the Player tag or any other relevant tag
        if (other.CompareTag("Player"))
        {
            // Trigger the pickup action
            Interact();
        }
    }

    // Pickup action logic
    public void Interact()
    {
        Debug.Log("Picked up: " + itemName);

        // Update GameManager's item count
        gameManager.IncrementItemCount(1);

        // Destroy the object after updating item count
        Destroy(gameObject); // Destroy the object when picked up
    }
}