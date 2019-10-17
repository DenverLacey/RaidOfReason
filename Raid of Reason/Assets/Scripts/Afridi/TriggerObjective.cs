using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    public delegate void OnTriggerEvents(TriggerObjective trigger);
    public OnTriggerEvents Triggers;
    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Player"))
        {
            objectiveManager.ObjectiveTriggered = true;
            Triggers(this);
            // This is to using the same trigger again
            this.gameObject.SetActive(false);
        }    
    }
}
