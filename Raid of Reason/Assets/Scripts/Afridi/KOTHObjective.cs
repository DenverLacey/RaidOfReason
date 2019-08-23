using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float currentTimer;

    [Tooltip("Centre of area")]
    public Vector3 centre;

    [Tooltip("Radius of area")]
    public float radius;

    public override void Awake()
    {
        currentTimer = timer;
    }

    public override void Update()
    {
        Collider[] hitPlayers = Physics.OverlapSphere(centre, radius, LayerMask.GetMask("Player"));
        if (hitPlayers.Length >= 3)
        {
            currentTimer -= Time.deltaTime;
        }
        IsDone();
    }

    public override bool HasFailed() { return false; }

    public override bool IsDone()
    {
        return currentTimer <= 0f;
    }
}
