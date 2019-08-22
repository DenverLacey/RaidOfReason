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
    private float timer;

    // Array to hold the number of enemies in game
    private EnemyData[] m_enemies;

    public override void Awake()
    {
        // Finds all gameobjects within the scene
        EnemyData[] m_enemies = GameObject.FindObjectsOfType<EnemyData>();
        timer = maxtimer;
        amount = m_enemies.Length;
        
    }

    public override void Update()
    {
        // Timer starts going down
        timer -= Time.deltaTime;

        // If the timer is greater than 0
        if (timer > 0)
        {
            // For each enemy in the array
            foreach (EnemyData data in m_enemies)
            {
                // if one of the enemies die
                if (data.Health <= 0)
                {
                    // Amount is decremented 
                    amount--;
                    data.OnDeath();
                }
            }
            // Check if the objective is done
            IsDone();
        }
        else
        {
            // Or else you failed the objective
            HasFailed();
        }
    }

    public override bool IsDone()
    {
        return amount <= 0;
    }

    public override bool HasFailed()
    {
        return timer <= 0;
    }
}
