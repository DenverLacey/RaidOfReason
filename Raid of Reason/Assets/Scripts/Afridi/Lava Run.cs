using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Lava Run")]
public class LavaRun : BaseObjective
{ 
    [Tooltip("The Objective Description")]
    public string description;
    [Tooltip("Name of the Objective")]
    public string name;

    private Lava lavaManager;
    private LavaSafeZone SafeZone;
    private GameManager manager;
    private float m_currentTimer;
    public GameObject SpawnPoint;
    public override void Awake()
    {
        lavaManager.m_timer = m_currentTimer;
        SafeZone = FindObjectOfType<LavaSafeZone>();
        lavaManager = FindObjectOfType<Lava>();
    }

    public override void Update() {  }

    public override string GrabTitle()
    {
        return name;
    }

    public override string GrabDescription()
    {
        return description;
    }

    public override bool HasFailed()
    {
        return manager.DeadPlayers.Count > 1 || m_currentTimer <= 0;
    }

    public override bool IsDone()
    {
        return SafeZone.m_KenronSafe == true && SafeZone.m_MachinaSafe == true && SafeZone.m_TheaSafe == true;
    }

    public override float Timer()
    {
        return m_currentTimer;
    }

    public override GameObject SpawnPoints()
    {
        return SpawnPoint;
    }
}
