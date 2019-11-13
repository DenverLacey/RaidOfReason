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
    [Tooltip("Player 2's cursor object")]
    private PauseCursor m_p2Cursor;

    [SerializeField]
    [Tooltip("Player 3's cursor object")]
    private PauseCursor m_p3Cursor;


    [SerializeField]
    [Tooltip("Character Scene Index")]
    private int m_characterSceneIndex;

    [SerializeField]
    [Tooltip("Scene to restart")]
    private int m_restartedScene;

    [HideInInspector]
    public bool m_isPaused { get; set; }
    private Vector3 m_P1InactivePosition;
    private Vector3 m_P2InactivePosition;
    private Vector3 m_P3InactivePosition;
    public GameObject m_pauseMenu;
    private GameObject m_playerHUD;

    private void Start()
    {
        m_pauseMenu = GameObject.Find("---PauseMenu---");
        m_pauseMenu.gameObject.SetActive(false);
        m_isPaused = false;
        m_playerHUD = GameObject.Find("---Stats---");
        m_playerHUD.SetActive(true);
        m_pauseMenu.SetActive(false);
        m_P1InactivePosition = m_p1Cursor.transform.position;
        m_P2InactivePosition = m_p2Cursor.transform.position;
        m_P3InactivePosition = m_p3Cursor.transform.position;

        m_p1Cursor.gameObject.SetActive(false);
        m_p2Cursor.gameObject.SetActive(false);
        m_p3Cursor.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(GameManager.Instance.AlivePlayers.Count == 0)
        {
            return;
        }
        else
        {
            if (XCI.GetButtonDown(XboxButton.Start, XboxController.First))
            {
                m_p1Cursor.SetController(1);
                if (!m_pauseMenu.activeInHierarchy && m_isPaused == false)
                {
                    m_playerHUD.SetActive(false);
                    m_p1Cursor.gameObject.SetActive(true);
                    Paused();
                }
                else if (m_pauseMenu.activeInHierarchy && m_isPaused == true)
                {
                    m_playerHUD.SetActive(true);
                    m_p1Cursor.gameObject.SetActive(false);
                    ContinueGame();
                }
            }

            if (XCI.GetButtonDown(XboxButton.Start, XboxController.Second))
            {
                m_p2Cursor.SetController(2);
                if (!m_pauseMenu.activeInHierarchy && m_isPaused == false)
                {
                    m_playerHUD.SetActive(false);
                    m_p2Cursor.gameObject.SetActive(true);
                    Paused();
                }
                else if (m_pauseMenu.activeInHierarchy && m_isPaused == true)
                {
                    m_playerHUD.SetActive(true);
                    m_p2Cursor.gameObject.SetActive(false);
                    ContinueGame();
                }
            }

            if (XCI.GetButtonDown(XboxButton.Start, XboxController.Third))
            {
                m_p3Cursor.SetController(3);
                if (!m_pauseMenu.activeInHierarchy && m_isPaused == false)
                {
                    m_playerHUD.SetActive(false);
                    m_p3Cursor.gameObject.SetActive(true);
                    Paused();
                }
                else if (m_pauseMenu.activeInHierarchy && m_isPaused == true)
                {
                    m_playerHUD.SetActive(true);
                    m_p3Cursor.gameObject.SetActive(false);
                    ContinueGame();
                }
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
        m_pauseMenu.SetActive(true);
    }

    public void ContinueGame()
    {
        m_isPaused = false;
        Time.timeScale = 1;
        m_pauseMenu.SetActive(false);
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
