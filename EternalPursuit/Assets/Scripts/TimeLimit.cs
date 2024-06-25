using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeLimit : MonoBehaviour
{
    [SerializeField] float currentTime;
    private float timeLimit;
    [SerializeField] TextMeshProUGUI countdownText;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
           
            currentTime -= Time.deltaTime;
        }
        else if(currentTime < 1)
        {
          Destroy(countdownText);
            //GameManager.Instance.gameStats.Deaths++;
            GameManager.Instance.lostState();
        }

       
        
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        countdownText.text = string.Format(" {0:0}:{1:00}", minutes, seconds);
    }

    public void AddTime(float amount)
    {
        currentTime += amount;
    }

    public float GetTime()
    {
        return currentTime;
    }
}
