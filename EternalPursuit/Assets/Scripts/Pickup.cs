using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    ItemStack items;

    public void SetItems(ItemStack items)
    {
        this.items = items;
        GenerateModel();
    }

    private void Start()
    {
        if(items != null)
            GenerateModel();
    }

    public void GenerateModel()
    {
        if(transform.childCount > 0) Destroy(transform.GetChild(0).gameObject);
        Instantiate(items.item.Prefab, transform);
    }

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