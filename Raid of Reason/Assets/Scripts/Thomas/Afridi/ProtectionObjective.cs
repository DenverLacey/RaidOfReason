﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Protect Me")]
public class ProtectionObjective : BaseObjective
{
    [Tooltip("Name of the Objective")]
    public string nameOfObject;
    [Tooltip("Health of the Object in protection")]
    public float health;
    [Tooltip("Time of Protection")]
    public float timer;
    [Tooltip("The Objective Description")]
    public string description;

    public GameObject ProtectObject { get; private set; }
    public string spawnPointName;
    private GameObject SpawnPoint;
    public float m_currentTimer;
    public float m_currentHealth;

    private void OnEnable()
    {
        ProtectObject = GameObject.Find(nameOfObject);
    }

    public override GameObject ActivatePortal()
    {
        return null;
    }

    public override void Init()
    {
        m_currentHealth = health;
        m_currentTimer = timer;
    }

    public override float Timer()
    {
        return m_currentTimer;
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= damage;
    }

    public override GameObject SpawnPoints()
    {
        return SpawnPoint;
    }

    public override string GrabDescription()
    {
        return description;
    }

    public override void Update()
    {
        m_currentTimer -= Time.deltaTime;
        if (HasFailed() == true && ProtectObject != null)
        {
            ProtectObject.SetActive(false);
        }
    }

    public override bool HasFailed()
    {
        return m_currentHealth <= 0;
    }

    public override bool IsDone()
    {
        return m_currentTimer <= 0 && m_currentHealth > 0;
    }
}
