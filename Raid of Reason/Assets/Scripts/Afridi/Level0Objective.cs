﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Tutorial Objective")]
public class Level0Objective : BaseObjective
{
    [Tooltip("Scene to transport to once on the portal")]
    public int buildIndex;

    [Tooltip("The Objective Description")]
    public string description;

    [Tooltip("Name of the Objective")]
    public string name;
    public string portalName;
    public string enemyZoneName;
    public string spawnPointName;
    private GameObject spawnPoint;
    private GameObject portal;
    private GameObject Zone;

    public override void Awake()
    {
        portal = GameObject.Find(portalName);
        if (portal != null)
        {
            portal.SetActive(false);
        }
        spawnPoint = GameObject.Find(spawnPointName);
        Zone = GameObject.Find(enemyZoneName);
    }

    public override GameObject SpawnPoints()
    {
        return spawnPoint;
    }

    public override float Timer()
    {
        return 0f;
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
       
    }

    public override GameObject ActivatePortal()
    {
        return portal;
    }

    public override bool IsDone()
    {
        bool spawnerExists = Zone.GetComponent<EnemyZone>().Enemies.Exists(e => e.Type == "Spawner");
        return !spawnerExists;
    }

    public override bool HasFailed()
    {
        return false;
    }
}