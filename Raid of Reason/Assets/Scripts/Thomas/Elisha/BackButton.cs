/*
 * Author: Elisha
 * Description: This script allows the players to go back to the main menu from the character select screen.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : InteractableUIElement
{
    public void OnPressed()
    {
        Application.Quit();
    }
}