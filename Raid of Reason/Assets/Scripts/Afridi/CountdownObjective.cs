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
    public string spawnPointName;
    public string enemyZoneName;
    private GameObject spawnPoint;
    private GameObject Zone;

    // Current Timer
    private float currentTimer;

    public override void Awake()
    {
        currentTimer = maxtimer;
    }

    public override GameObject SpawnPoints()
    {
        return spawnPoint;
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
        spawnPoint = GameObject.Find(spawnPointName);
        Zone = GameObject.Find(enemyZoneName);
        // Timer starts going down
        currentTimer -= Time.deltaTime;
    }

    public override bool IsDone()
    {
        bool spawnerExists = Zone.GetComponent<EnemyZone>().Enemies.Exists(e => e.Type == "Spawner");
        return !spawnerExists;
    }

    public override bool HasFailed()
    {
        return currentTimer <= 0;
    }
}
