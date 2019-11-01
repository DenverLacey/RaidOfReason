using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Objectives/Fight Me")]
public class BossObjective : BaseObjective
{
    [Tooltip("The Boss That The Players have to Slay")]
    public GameObject Boss;
    [Tooltip("The Boss Health Bar that is displayed")]
    public  GameObject bosshealthBar;
    [Tooltip("The Objective Description")]
    public string description;
    [Tooltip("Name of the Objective")]
    public string name;
    public string spawnPointName;

    private GameManager manager;
    public GameObject SpawnPoint;

    public override void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        Boss = GameObject.Find("Boss");
        bosshealthBar = GameObject.Find("Boss_Health");
        SpawnPoint = GameObject.Find(spawnPointName);
    }

    public override void Update()
    {
        if (Boss != null )
            bosshealthBar.GetComponent<Image>().fillAmount = Boss.GetComponent<EnemyData>().Health / Boss.GetComponent<EnemyData>().MaxHealth;
    }

    public override GameObject ActivatePortal()
    {
        return null;
    }
    public override GameObject SpawnPoints()
    {
        return SpawnPoint;
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
        //return Boss.GetComponent<EnemyData>().Health <= 0;
        return Boss == null;
    }
}
