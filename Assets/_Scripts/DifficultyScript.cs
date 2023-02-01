using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyScript : MonoBehaviour
{
    private int seconds;
    private float timer;
    public static int spawnedAppleCount;
    public GameObject apple, activeApple;
    private bool isCreated;

    private void Start()
    {
        spawnedAppleCount = 1;
    }

    void Update()
    {
        Timer();
        DifficultyManagement();
        print(seconds);
    }
    
    void Timer()
    {
        timer = 0;
        timer += Time.timeSinceLevelLoad;
        seconds = Convert.ToInt32(timer);
    }
    
    
    private void DifficultyManagement()
    {
        if (seconds > 9 && ((seconds % 10) == 0))
        {
            for (spawnedAppleCount = 1; spawnedAppleCount < 5; spawnedAppleCount++)
            {
                if (!isCreated)
                {
                    Instantiate(apple, activeApple.transform.parent);
                    isCreated = true;
                }
                
            }
        }
    }
}
