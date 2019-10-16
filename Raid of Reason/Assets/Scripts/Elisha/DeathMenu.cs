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

    private Vector3 m_p1InactivePosition;
    private bool m_isDeath = false;
    public GameObject m_DeathMenu;
    private GameObject m_miniMap;

    private void Start()
    {
        m_p1Cursor.SetController(1);
        m_p1InactivePosition = transform.position;
        m_DeathMenu.SetActive(false);
        m_miniMap = GameObject.Find("Minimap_Outline");
    }

    public void DeathScreen()
    {
        Time.timeScale = 0.0f;
        m_isDeath = true;
        m_p1Cursor.gameObject.SetActive(true);
        m_DeathMenu.SetActive(true);
        m_miniMap.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(m_sceneToRestartIndex, LoadSceneMode.Single);
    }

    public void Quit()
    {
        LevelManager.LoadLevel(0);
    }
}
