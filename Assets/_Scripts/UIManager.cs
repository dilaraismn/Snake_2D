using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject startUI;
    public static bool isGameStarted;

    private void Start()
    {
        isGameStarted = false;
        startUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Button_Start()
    {
        isGameStarted = true;
        startUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
