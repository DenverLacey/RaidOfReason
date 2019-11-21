/*
* Author: Elisha_Anagnostakis
* Description: This script spawns in the death menu when all players are dead and handles all the button 
* transitions that the menu offers to the player.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player 1's cursor object")]
    private PauseCursor m_p1Cursor;

    [SerializeField]
    [Tooltip("Scene to restart when pressed")]
    private int m_sceneToRestartIndex;
    // Player spawn position for cursor
    private Vector3 m_p1InactivePosition;

    /// <summary>
    /// flag to check if death screen is active or not
    /// </summary>
    public bool m_isDeath { get; set; }
    
    private GameObject m_playerHUD;
    private GameObject m_objectiveHUD;
    private GameObject m_DeathMenu;

    /// <summary>
    /// Automatically finds all the gameobjects by name within the inspector.
    /// </summary>
    private void Start()
    {
        m_objectiveHUD = GameObject.Find("---Objectives---");
        m_DeathMenu = GameObject.Find("---EndMenu---");
        m_DeathMenu.SetActive(false);
        m_isDeath = false;
        m_p1Cursor.SetController(1);
        m_p1InactivePosition = transform.position;
        m_playerHUD = GameObject.Find("---Stats---");
        m_playerHUD.SetActive(true);
    }

    /// <summary>
    /// When the death screen gets set to active it spawns in the player cursor,
    /// pauses the game and sets all HUD to false.
    /// </summary>
    public void DeathScreen()
    {
        Time.timeScale = 0.0f;
        m_isDeath = true;
        m_playerHUD.SetActive(false);
        m_objectiveHUD.SetActive(false);
        m_p1Cursor.gameObject.SetActive(true);
        m_DeathMenu.SetActive(true);
    }

    /// <summary>
    /// When clicked it will restart the level index by using the level manager to 
    /// dynamically fade to the chosen level index.
    /// </summary>
    /// <param name="levelIndex"></param>
    public void ClickedRestart(int levelIndex)
    {
        Time.timeScale = 1;
        m_DeathMenu.SetActive(false);
        LevelManager.FadeLoadLevel(levelIndex);
    }

    /// <summary>
    /// When clicked it will dynamically fade to the main menu of the game.
    /// </summary>
    public void ClickQuit()
    {
        Time.timeScale = 1;
        m_DeathMenu.SetActive(false);
        LevelManager.FadeLoadLevel(0);
    }
}