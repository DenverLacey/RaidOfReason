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
        if (Utility.TagIsPlayerTag(other.tag))
        {
            objectiveManager.ObjectiveTriggered = true;
            // This is to using the same trigger again
            this.gameObject.SetActive(false);
        }    
    }
}
