using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EnemyManager enemyManager;

    public GameObject Player;
    public Vector3 LastKnownPosition;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject InventoryMenu;
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TMP_Text enemycountText;
    [SerializeField] TMP_Text itemcountText;

    public Image PlayerHPBar;
    public GameObject playerDFlash;
    public ItemSlot[] items;

    public bool pause;
    public bool inventory;
    int enemyCount;
    int itemCount;

    [SerializeField] bool IgnoreLoss = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        Instance = this;

        enemyManager = GetComponent<EnemyManager>();

        InventoryMenu.SetActive(false);
        GameObject Inventory = GameObject.Find("Inventory");

            resumeState();

        
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
             
            TogglePauseMenu();
        }

        if (Input.GetButtonDown("Inventory"))
        {
             
            ToggleInventoryMenu();
        }
    }

    void TogglePauseMenu()
    {
        if (activeMenu == null)
        {
            pauseState();
            activeMenu = pauseMenu;
            activeMenu.SetActive(pause);
        }
        else if (activeMenu == pauseMenu)
        {
            resumeState();
        }
    }

    void ToggleInventoryMenu()
    {
        if (activeMenu == null)
        {
            
            inventoryState();
            activeMenu = InventoryMenu;
            activeMenu.SetActive(inventory);
        }
        else if (activeMenu == InventoryMenu)
        {
             
            resumeState();
        }
    }





    public void IncrementItemCount(int amount)
    {
        itemCount += amount;

        if (itemcountText != null)
        {
            itemcountText.text = itemCount.ToString("F0");
            Debug.Log("Updated item count text to: " + itemcountText.text);
        }
        else
        {
            Debug.LogError("ItemCountText is null when trying to update item count.");
        }
    }

    public void AddItem(string itemName, int itemAmount, string itemDescription)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].isFull && items[i].itemName == itemName )
            {
                items[i].itemAmount += itemAmount;
                
                Debug.Log("Stacked itemName = " + itemName + ", quantity = " + itemAmount);
                items[i].isFull = true;
                
                return;
            }
        }
            for (int i = 0; i < items.Length; i++)
            {
                if (!items[i].isFull )
                {
                    items[i].AddItem(itemName, itemAmount, itemDescription);
                    Debug.Log("Added new itemName = " + itemName + ", quantity = " + itemAmount);
                    return;
                }
            }


        }
    

    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        enemycountText.text = enemyCount.ToString("F0");

        if(enemyCount <= 0)
        {
            pauseState();
            activeMenu = winMenu;
            activeMenu.SetActive(pause);
        }

    }

    public void inventoryState()
    {
        if (!pause)
        {
            inventory = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
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

    public void pauseState()
    {
        if (!inventory)
        {
            pause = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void resumeState()
    {
        pause = false;
        inventory = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(activeMenu != null)
        {
            activeMenu.SetActive(pause);
            activeMenu = null;
        }
    }

    public void lostState()
    {
        if (IgnoreLoss) return;

        pauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(pause);
    }
}
