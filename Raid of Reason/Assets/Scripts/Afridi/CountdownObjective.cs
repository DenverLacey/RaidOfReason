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

    // Amount of enemies in the game
    private int amount;

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
        amount = m_enemies.Count;
    }

    public void Reset()
    {
        m_enemies.Clear();
        amount = 0;
    }

    public override float Timer()
    {
        return currentTimer;
    }

    public override void Update()
    {
        // Timer starts going down
        currentTimer -= Time.deltaTime;

        // If the timer is greater than 0
        if (currentTimer > 0)
        {
            // For each enemy in the array
            foreach (EnemyData data in m_enemies)
            {
                // if one of the enemies die
                if (data.Health <= 0)
                {
                    // Amount is decremented 
                    amount--;
                }
            }
        }
    }

    public override bool IsDone()
    {
        return amount <= 0;
    }

    public override bool HasFailed()
    {
        return currentTimer <= 0;
    }
}
