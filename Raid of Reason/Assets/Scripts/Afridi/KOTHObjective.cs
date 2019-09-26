using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Author: Afridi Rahim
 * 
 * Summary:
 * This Script creates a King Of The Hill Objective 
 */
[CreateAssetMenu(menuName = "Objectives/King Of The Hill")]
public class KOTHObjective : BaseObjective
{
    [Tooltip("Duration of The Objective")]
    public float timer;

    [Tooltip("Name of the Objective")]
    public string name;

    [Tooltip("The Objective Description")]
    public string description;

    // Timer of the objective
    private float currentTimer;

    [Tooltip("Centre of area")]
    public Vector3 centre;

    [Tooltip("Radius of area")]
    public float radius;

    public override void Awake()
    {
        currentTimer = timer;
    }

    public override float Timer()
    {
        return currentTimer;
    }

    public override string GrabTitle()
    {
        return name;
    }

    public override void Update()
    {
		var alivePlayers = GameManager.Instance.AlivePlayers;
		int playersOnHill = 0;

		foreach (var player in alivePlayers)
		{
			Vector3 playerPos = new Vector3(player.transform.position.x, centre.y, player.transform.position.z);
			float sqrDistance = (centre - playerPos).sqrMagnitude;

			if (sqrDistance <= radius * radius)
				playersOnHill++;
		}

		if (playersOnHill == alivePlayers.Count)
			currentTimer -= Time.deltaTime;
    }

    public override bool HasFailed() { return false; }

    public override bool IsDone()
    {
        return currentTimer <= 0f;
    }

    public override string GrabDescription()
    {
        return description;
    }
}
