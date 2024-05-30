using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public EnemyManager enemyManager;

    public GameObject Player;
    public Player playerScript;
    public Vector3 LastKnownPosition;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject InventoryMenu;
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TMP_Text enemycountText;
    [SerializeField] TMP_Text itemcountText;
    [SerializeField] TMP_Text ammocountText;

    public GameObject playerSpawnPos;
    public GameObject checkpointPopup;

    public Image PlayerHPBar;
    public GameObject playerDFlash;
    public ItemSlot[] items;
    public ItemSO[] itemSOs;
    
    
    public bool pause;
    public bool inventory;
    int enemyCount;
    int itemCount;
    int ammoCount;

    [SerializeField] bool IgnoreLoss = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
        Instance = this;

        enemyManager = GetComponent<EnemyManager>();
        playerScript = Player.GetComponent<Player>();

        InventoryMenu.SetActive(false);

        if (ammocountText == null)
        {
            ammocountText = GameObject.Find("AmmoCountText").GetComponent<TMP_Text>();
            if (ammocountText == null)
            {
                Debug.LogError("AmmoCountText component not found.");
            }
        }



        resumeState();

        

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject Inventory = GameObject.Find("Inventory");
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

    public bool UseItem(string itemName)
    { 
        for(int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName) 
            {
                bool usable = itemSOs[i].UseItem();
                return usable;
                
            }
            
        }
        return false;
    }

    public int AddItem(string itemName, int itemAmount, string itemDescription)
    {
        // Check for existing slots with the same item and not full
        foreach (var itemSlot in items)
        {
            if (itemSlot.itemName == itemName && !itemSlot.isFull)
            {
                itemAmount = itemSlot.AddItem(itemName, itemAmount, itemDescription);
                if (itemAmount == 0)
                {
                    return 0;
                }
            }
        }

        // If there are leftover items, check for an empty slot
        foreach (var itemSlot in items)
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

    public void UpdateAmmoCount(int ammoCount)
    {
        if (ammocountText != null)
        {
            ammocountText.text = ammoCount.ToString("F0");
        }
        else
        {
            Debug.LogError("AmmoCountText is null when trying to update item count.");
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
