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



    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

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
    public void AddItem(string itemName, int itemAmount)
    {
        this.itemName = itemName;
        this.itemAmount = itemAmount;
        isFull = true;
        quantityText.text = itemAmount.ToString();
        quantityText.enabled = true;

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
        if (gameManager != null)
        {
            gameManager.DeselectAllSlots();
        }
        if (selectedShader != null)
        {
            selectedShader.SetActive(true);
        }
        thisItemSelected = true;
    }
    public void OnRightClick()
    {

    }
}
