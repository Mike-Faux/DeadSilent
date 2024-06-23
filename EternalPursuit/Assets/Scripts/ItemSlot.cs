using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

public class ItemSlot : MonoBehaviour, IPointerClickHandler

{
    public ItemSO item;
    [SerializeField]
     private int maxNumberOfItems;
    public int itemAmount;
    public bool isFull;


    [SerializeField]
     private TMP_Text quantityText;

    [SerializeField]
     private Image itemImage;

    

    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    // Start is called before the first frame update
    public int AddItem(ItemStack item)
    {
        if (item != null && (itemAmount >= maxNumberOfItems || item.item != this.item))
            return itemAmount;

        if(item == null) this.item = item.item;

        itemAmount += item.count;

        if (itemAmount > maxNumberOfItems)
        {
            int extraItems = itemAmount - maxNumberOfItems;
            itemAmount = maxNumberOfItems;
            UpdateUI();
            return extraItems;
        }
         else
        {
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
            bool usable =  GameManager.Instance.UseItem(item);
            if (usable)
            {
                itemAmount -= 1;
                quantityText.text = itemAmount.ToString();
                if (itemAmount <= 0)
                
                    EmptySlot();
                
            }
           
        }


        else
        {
        //GameManager.Instance.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
        ItemDescriptionNameText.text = item.Name;
        ItemDescriptionText.text = item.Description;
         }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        item = null;
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
    }
    public void OnRightClick()
    {

    }
}
