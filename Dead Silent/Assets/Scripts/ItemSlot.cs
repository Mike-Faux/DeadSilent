using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
    
{
    public string itemName;
    public int itemAmount;
    public bool isFull;



    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    // Start is called before the first frame update
    public void AddItem(string itemName, int itemAmount)
    {
        this.itemName = itemName;
        this.itemAmount = itemAmount;
        isFull = true;
        quantityText.text = itemAmount.ToString();
        quantityText.enabled = true;
        
    }
}
