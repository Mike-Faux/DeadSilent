using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour, IInteractable
{
    public string itemName;

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
        Destroy(gameObject); // Destroy the object when picked up
        // You can add additional logic here, such as adding the item to the player's inventory
    }
}