﻿using System.Collections;
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
    private bool m_isDeath;
    private GameObject m_playerHUD;
    public GameObject m_DeathMenu;

    private void Start()
    {
        m_DeathMenu = GameObject.Find("---EndMenu---");
        m_DeathMenu.SetActive(false);
        m_isDeath = false;
        m_p1Cursor.SetController(1);
        m_p1InactivePosition = transform.position;
        m_playerHUD = GameObject.Find("---Stats---");
        m_playerHUD.SetActive(true);
    }

    public void DeathScreen()
    {
        Time.timeScale = 0.0f;
        m_isDeath = true;
        m_playerHUD.SetActive(false);
        m_p1Cursor.gameObject.SetActive(true);
        m_DeathMenu.SetActive(true);
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
