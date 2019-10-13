using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager objectiveManager;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"));
        {
            objectiveManager.ObjectiveTriggered = true;
        }    
    }
}
