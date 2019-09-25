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
    [Tooltip("First Scene Index")]
    private int m_FirstSceneIndex;

    private Vector3 m_p1InactivePosition;
    private bool m_isPaused = false;
    public GameObject m_pauseMenu;

    private void Start()
    {
        m_p1Cursor.SetController(1);
        m_p1InactivePosition = transform.position;
        m_pauseMenu.SetActive(false);
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
    }

    public void ContinueGame()
    {
        m_isPaused = false;
        Time.timeScale = 1;
        m_p1Cursor.gameObject.SetActive(false);
        m_pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(m_FirstSceneIndex, LoadSceneMode.Single);
    }

    public void Quit()
    {
        LevelManager.LoadLevel(0);
    }
}
