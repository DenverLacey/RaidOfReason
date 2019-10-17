using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    private BarrierManager Barriers;
    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        Barriers = FindObjectOfType<BarrierManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (Utility.TagIsPlayerTag(other.tag))
        {
            Barriers.ManageBarriers();
            objectiveManager.ObjectiveTriggered = true;
            this.gameObject.SetActive(false);
        }    
    }
}
