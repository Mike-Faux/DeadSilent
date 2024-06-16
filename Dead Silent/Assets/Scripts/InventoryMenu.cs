using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    public int AddItem(ItemStack items)
    {
        // Check for existing slots with the same item and not full
        foreach (var itemSlot in this.items)
        {
            if (itemSlot.item == items.item)
            {
                itemAmount = itemSlot.AddItem(itemName, itemAmount, itemDescription);
                if (itemAmount == 0)
                {
                    return 0;
                }
            }
        }

        // If there are leftover items, check for an empty slot
        foreach (var itemSlot in this.items)
        {
            if (string.IsNullOrEmpty(itemSlot.itemName))
            {
                itemAmount = itemSlot.AddItem(itemName, itemAmount, itemDescription);
                return itemAmount;
            }
        }

        // Return the amount that couldn't be added
        return itemAmount;
    }

    
    public void DeselectAllSlots()
    {
        for (int i = 0; i < items.Length; i++)
        {

            items[i].selectedShader.SetActive(false);
            items[i].thisItemSelected = false;
            items[i].ItemDescriptionNameText.text = items[i].itemName;
            items[i].ItemDescriptionText.text = items[i].itemDescription;
        }
    }
    /**/
}
