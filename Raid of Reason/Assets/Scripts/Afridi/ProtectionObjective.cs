using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim
 * Description: Time Based Protection Objective
 * Last Edited: 15/11/2019
 */
[CreateAssetMenu(menuName = "Objectives/Protect The Crystal")]
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
    private float m_currentTimer;
    public float currentHealth;

    #region Objective Setup
    private void OnEnable()
    {
        // Finds the Crystal
        ProtectObject = GameObject.Find(nameOfObject);
    }

    public override void Init()
    {
        currentHealth = health;
        m_currentTimer = timer;

		MusicManager.Transition(MusicManager.MusicType.LVL_2_PHASE_2);
    }

    public override float Timer()
    {
        return m_currentTimer;
    }

    public void TakeDamage(float damage)
    {
        // Enables the crystal to take damage
        currentHealth -= damage;
    }

    public override string GrabDescription()
    {
        return description;
    }

    public override void Update()
    {
        m_currentTimer -= Time.deltaTime;
        if (Failed() == true && ProtectObject != null)
        {
            ProtectObject.SetActive(false);
        }
    }

    public override bool Failed()
    {
        // Failure Requirments: Crystal Dies
        return currentHealth <= 0;
    }

    public override bool Completed()
    {
        // Completion Requirements: Timer is 0 and health is above 0
        return m_currentTimer <= 0 && currentHealth > 0;
    }
    #endregion
}
