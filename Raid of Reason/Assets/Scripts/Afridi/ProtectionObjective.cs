using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Protect Me")]
public class ProtectionObjective : BaseObjective
{
    [Tooltip("The Object that you are protecting")]
    public GameObject ProtectObject;
    [Tooltip("Health of the Object in protection")]
    public float health;
    [Tooltip("Time of Protection")]
    public float timer;
    [Tooltip("The Objective Description")]
    public string description;
    [Tooltip("Name of the Objective")]
    public string name;

    public GameObject SpawnPoint;
    private float m_currentTimer;
    private float m_currentHealth;

    public override void Awake()
    {
        m_currentHealth = health;
        m_currentTimer = timer;
    }

    public override float Timer()
    {
        return m_currentTimer;
    }

    public override GameObject SpawnPoints()
    {
        return SpawnPoint;
    }

    public override string GrabDescription()
    {
        return description;
    }

    public override string GrabTitle()
    {
        return name;
    }

    public override void Update()
    {
        m_currentTimer -= Time.deltaTime;
        if (HasFailed() == true)
        {
            ProtectObject.SetActive(false);
        }
    }

    public override bool HasFailed()
    {
        return m_currentTimer <= 0 || health <= 0;
    }

    public override bool IsDone()
    {
        return health < 0;
    }
}
