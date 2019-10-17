﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private ObjectiveManager m_objectiveManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    public static CheckPointManager Instance { get; private set; }

    [Header("--Respawn Values--")]

    [SerializeField]
    [Tooltip("How long it takes for the player to respwn.")]
    private float m_spawnDelay;

    public void RespawnToCheckpoint()
    {
        foreach (BaseCharacter player in GameManager.Instance.DeadPlayers)
        {
            if (m_objectiveManager.m_currentObjective.name == m_objectiveManager.m_objectives[0].name)
            {
                player.gameObject.transform.position = m_objectiveManager.m_currentObjective.SpawnPoints().transform.position;
                player.gameObject.SetActive(true);
                player.playerState = BaseCharacter.PlayerState.ALIVE;
                player.m_currentHealth = player.m_maxHealth;
            }
        }
    }

    public void InvokeRespawn()
    {
        Invoke("RespawnToCheckpoint", m_spawnDelay);
    }
}