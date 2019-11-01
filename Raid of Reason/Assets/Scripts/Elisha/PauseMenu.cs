/*
 * Author: Elisha
 * Description: This script checks on update if the player decides to pause or unpause the game at any moment.
 *              Also handles what happens if the restart, quit or character select button gets pressed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using XboxCtrlrInput;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Player 1's cursor object")]
    private PauseCursor m_p1Cursor;

    [SerializeField]
    [Tooltip("Character Scene Index")]
    private int m_characterSceneIndex;

    [SerializeField]
    [Tooltip("Scene to restart")]
    private int m_restartedScene;

    [HideInInspector]
    public bool m_isPaused { get; set; }
    private Vector3 m_p1InactivePosition;
    public GameObject m_pauseMenu;
    private GameObject m_miniMap;

    private void Start()
    {
        m_isPaused = false;
        m_p1Cursor.SetController(1);
        m_p1InactivePosition = transform.position;
        m_pauseMenu.gameObject.SetActive(false);
        m_miniMap = GameObject.Find("Minimap_Outline");
    }

    public void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.First))
        {
            if (!m_pauseMenu.activeInHierarchy && m_isPaused == false)
            {
                Paused();
            }
            else if (m_pauseMenu.activeInHierarchy && m_isPaused == true)
            {
                ContinueGame();
            }
        }
        else if (XCI.GetButtonDown(XboxButton.Start, XboxController.Second))
        {
            if (!m_pauseMenu.activeInHierarchy && m_isPaused == false)
            {
                Paused();
            }
            else if (m_pauseMenu.activeInHierarchy && m_isPaused == true)
            {
                ContinueGame();
            }
        }
        else if (XCI.GetButtonDown(XboxButton.Start, XboxController.Third))
        {
            if (!m_pauseMenu.activeInHierarchy && m_isPaused == false)
            {
                Paused();
            }
            else if (m_pauseMenu.activeInHierarchy && m_isPaused == true)
            {
                ContinueGame();
            }
        }
    }

    public void CharacterSelection()
    {
        SceneManager.LoadScene(m_characterSceneIndex, LoadSceneMode.Single);
    }

    public void Paused()
    {
        Time.timeScale = 0.0f;
        m_isPaused = true;
        m_p1Cursor.gameObject.SetActive(true);
        m_pauseMenu.SetActive(true);
        //m_miniMap.SetActive(false);
    }

    public void ContinueGame()
    {
        m_isPaused = false;
        Time.timeScale = 1;
        m_p1Cursor.gameObject.SetActive(false);
        m_pauseMenu.SetActive(false);
        //m_miniMap.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(m_restartedScene, LoadSceneMode.Single);
    }

    public void Quit()
    {
        LevelManager.LoadLevel(0);
    }
}
