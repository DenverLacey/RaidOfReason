using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjective : MonoBehaviour
{
    [HideInInspector]
    public bool isCompleted = false;

    [Tooltip("KOTM Objective")]
    public KOTHObjective KOTMObjective;

    protected void Update()
    {
        isDone();
    }

    protected void isDone() {
        if (isCompleted) {
            // Move to Next Scene
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           
           
        }       
    }
}
