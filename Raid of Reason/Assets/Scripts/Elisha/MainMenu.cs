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
        LevelManager.FadeToLevel(1);
       // StartCoroutine(TimeToLoad(1));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator TimeToLoad(int duration)
    {
        yield return new WaitForSeconds(duration);
        LevelManager.LoadNextLevel();
    }
}