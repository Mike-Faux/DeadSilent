using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector]
    public GameStats gameStats;
    public EnemyManager enemyManager;

    public GameObject Player;
    public Player playerScript;
    public Vector3 LastKnownPosition;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject InventoryMenu;
    [SerializeField] public GameObject activeMenu;
    [SerializeField] public GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TMP_Text enemycountText;
    [SerializeField] TMP_Text itemcountText;

    [SerializeField] TMP_Text WeaponNameText;
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



    [SerializeField] bool IgnoreLoss = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        Instance = this;

        GameObject gsgo = GameObject.FindGameObjectWithTag("GameStats");
        if (gsgo == null)
        {
            gsgo = new()
            {
                name = "GameStats",
                tag = "GameStats"
            };
            gameStats = gsgo.AddComponent<GameStats>();
            DontDestroyOnLoad(gsgo);
        }
        else
        {
            gameStats = gsgo.GetComponent<GameStats>();
        }

        playerScript = Player.GetComponent<Player>();

        resumeState();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");

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


    public bool UseItem(ItemSO item)
    { 
        for(int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i] == item) 
            {
                bool usable = itemSOs[i].UseItem();
                return usable;
                
            }
            
        }
        return false;
    }



    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        enemycountText.text = enemyCount.ToString("F0");

        //if (enemyCount == 0)
        //{
        //    pauseState();
        //    activeMenu = winMenu;
        //    activeMenu.SetActive(pause);
        //}
        //

    }

    public void UpdateWeaponName(string name)
    {
        WeaponNameText.text = name;
    }

    public void UpdateAmmoCount(int ammoCount, int ammoMax)
    {
        ammocountText.text = $"{ammoCount:00}/{ammoMax:00}";
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
