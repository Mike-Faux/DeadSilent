using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    [SerializeField]
    ItemStack items;



    // Called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the Player tag or any other relevant tag
        if (other.CompareTag("Player"))
        {
            // Trigger the pickup action
           int leftOverItems = other.GetComponent<Player>().inventory.AddItems(items);
           if(leftOverItems <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                items.count = leftOverItems;
            }
        }
    }
}