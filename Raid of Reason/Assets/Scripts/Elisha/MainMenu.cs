using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.LoadNextLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }
}