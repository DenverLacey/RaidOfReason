using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Objectives/Fight Me")]
public class BossObjective : BaseObjective
{
    [Tooltip("The Boss That The Players have to Slay")]
    public EnemyData Boss;
    [Tooltip("The Boss Health Bar that is displayed")]
    public Image bosshealthBar;
    [Tooltip("The Objective Description")]
    public string description;
    [Tooltip("Name of the Objective")]
    public string name;

    private GameManager manager;

    public override void Awake()
    {
        manager = FindObjectOfType<GameManager>();
    }

    public override void Update()
    {
        bosshealthBar.fillAmount = Boss.Health / Boss.MaxHealth;
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

    public override bool HasFailed()
    {
        return manager.DeadPlayers.Count > 3;
    }

    public override bool IsDone()
    {
        return Boss.Health <= 0;
    }
}
