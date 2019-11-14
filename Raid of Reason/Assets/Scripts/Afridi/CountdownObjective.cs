using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim
 * Description: Kill Based Objective
 * Last Edited: 15/11/2019
 */
[CreateAssetMenu(menuName = "Objectives/Countdown To Destruction")]
public class CountdownObjective : BaseObjective
{
    [Tooltip("Amount of time players have till Cave in")]
    public float maxtimer;

    [Tooltip("The Objective Description")]
    public string description;

    [Tooltip("Name of the Zone that the Objective Uses")]
    public string enemyZoneName;

    private GameObject Zone;
    private float currentTimer;

    #region Objective Setup
    public override void Init()
    {
        // Intialisation
        currentTimer = maxtimer;
        Zone = GameObject.Find(enemyZoneName);
    }

    public override float Timer()
    {
        return currentTimer;
    }

    public override string GrabDescription()
    {
        return description;
    }

    public override void Update()
    {
        Zone = GameObject.Find(enemyZoneName);
        // Timer starts going down
        currentTimer -= Time.deltaTime;
    }

    public override bool Completed()
    {
        // Completion Requirements: If all spawners in that zone are dead
        bool spawnerExists = Zone.GetComponent<EnemyZone>().Enemies.Exists(e => e.Type == "Spawner");
        return !spawnerExists;
    }

    public override bool Failed()
    {
        // Failure Requirements: If the timer has reached 0 
        return currentTimer <= 0;
    }
    #endregion 
}
