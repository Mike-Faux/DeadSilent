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
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;
    [SerializeField] TMP_Text enemycountText;
    [SerializeField] TMP_Text ItemcountText;

    public Image PlayerHPBar;
    public GameObject playerDFlash;

    public bool pause;
    int enemyCount;
    int ItemCount; 

    [SerializeField] bool IgnoreLoss = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        Instance = this;

        enemyManager = GetComponent<EnemyManager>();
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
            if(activeMenu == null)
            {
                pauseState();
                activeMenu = pauseMenu;
                activeMenu.SetActive(pause);
            }
            else if(activeMenu == pauseMenu)
            {
                resumeState();
            }
        }
    }

     public void IncrementItemCount(int amount)
     {
         ItemCount += amount;
         ItemcountText.text += ItemCount.ToString("F0");
         
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

    public void pauseState()
    {
        pause = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void resumeState()
    {
        pause = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(pause);
        activeMenu = null;
    }

    public void lostState()
    {
        if (IgnoreLoss) return;

        pauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(pause);
    }
}
