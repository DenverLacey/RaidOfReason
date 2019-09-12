using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim
 * 
 * Summary:
 * This Script creates a Countdown Objective 
 */
[CreateAssetMenu(menuName = "Objectives/Countdown To Destruction")]
public class CountdownObjective : BaseObjective
{
    [Tooltip("Amount of time players have till Cave in")]
    public float maxtimer;

    [Tooltip("The Objective Description")]
    public string description;

    [Tooltip("Name of the Objective")]
    public string name;

    // Current Timer
    private float currentTimer;

    // Array to hold the number of enemies in game
    private List<EnemyData> m_enemies = new List<EnemyData>();

    public override void Awake()
    {
        // Finds all gameobjects within the scene
        Reset();
        m_enemies.AddRange(FindObjectsOfType<EnemyData>());
        currentTimer = maxtimer;
    }

    public void Reset()
    {
        m_enemies.Clear();
    }

    public override float Timer()
    {
        return currentTimer;
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
        // Timer starts going down
        currentTimer -= Time.deltaTime;

        // If the timer is greater than 0
        if (currentTimer > 0)
        {
            m_enemies.RemoveAll(e => !e);
        }
    }

    public override bool IsDone()
    {
        return m_enemies.Count == 0;
    }

    public override bool HasFailed()
    {
        return currentTimer <= 0;
    }
}
