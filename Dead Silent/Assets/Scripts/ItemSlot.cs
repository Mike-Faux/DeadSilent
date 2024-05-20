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
     private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    private GameManager gameManager;

    private void Start()
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
    public void AddItem(string itemName, int itemAmount, string itemDescription)
    {
        this.itemName = itemName;
        this.itemAmount += itemAmount;
        this.itemDescription = itemDescription;


        isFull = false;
          
        
        quantityText.text = itemAmount.ToString();
        quantityText.enabled = true;
        

        UpdateUI();

    }

    public void UpdateUI()
    {
       
        // Update itemImage.sprite or other UI elements as needed
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
        gameManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = itemName;
        ItemDescriptionText.text = itemDescription;
    }
    public void OnRightClick()
    {

    }
}
