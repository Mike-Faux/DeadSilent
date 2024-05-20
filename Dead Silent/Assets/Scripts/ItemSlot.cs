using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler

{
    public string itemName;
    public int itemAmount;
    public bool isFull;
    public string itemDescription;
    [SerializeField]
     private int maxNumberOfItems;


    [SerializeField]
     private TMP_Text quantityText;

    [SerializeField]
     private Image itemImage;

    

    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            // GameManager component found
            Debug.Log("GameManager component found successfully.");
        }
        else
        {
            // GameManager component not found
            Debug.LogError("GameManager component not found in the hierarchy upwards.");
        }
    }


    // Start is called before the first frame update
    public int AddItem(string itemName, int itemAmount, string itemDescription)
    {
        if (isFull && this.itemName != itemName)
            return itemAmount;

        this.itemName = itemName;
        this.itemDescription = itemDescription;


        int totalAmount = this.itemAmount + itemAmount;

        if (totalAmount >= maxNumberOfItems)
        {
            int extraItems = totalAmount - maxNumberOfItems;
            this.itemAmount = maxNumberOfItems;
            isFull = true;
            UpdateUI();
            return extraItems;
        }
         else
    {
        this.itemAmount = totalAmount;
        if (this.itemAmount == maxNumberOfItems)
            isFull = true;

        UpdateUI();
        return 0;
    }
    }

    private void UpdateUI()
    {
        quantityText.text = itemAmount.ToString();
        quantityText.enabled = itemAmount > 0;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    public void OnLeftClick()
    {
        if (thisItemSelected)
        {
            bool usable =  gameManager.UseItem(itemName);
            if (usable)
            {
                this.itemAmount -= 1;
                quantityText.text = this.itemAmount.ToString();
                if (this.itemAmount <= 0)
                
                    EmptySlot();
                
            }
           
        }


        else
        {
        gameManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
         }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemDescription = "";
        itemName = "";
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
    }
    public void OnRightClick()
    {

    }
}
