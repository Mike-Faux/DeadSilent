using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Player;
    public Vector3 LastKnownPosition;

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    public Image playerHP;
    

    public bool pause;
    int enemyCount;

    private void Awake()
    {
        Instance = this;
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

    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;

        if(enemyCount <= 0)
        {
            pauseState();
            activeMenu = winMenu;
            activeMenu.SetActive(pause);
        }

    }
    public void pauseState()
    {
        pause = !pause;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void resumeState()
    {
        pause = !pause;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(pause);
        activeMenu = null;
        
    }
    public void lostState()
    {
        pauseState();
        activeMenu = loseMenu;
        activeMenu.SetActive(pause);
    }
}
