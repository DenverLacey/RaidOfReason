/*
 * Author: Elisha
 * Description: This is a small script which holds data for two functions to start the game and quit the application.
 */

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