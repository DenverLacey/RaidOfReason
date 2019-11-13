using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    public GameObject respawnPoint;

    private bool playerHere = false;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (Utility.TagIsPlayerTag(other.tag))
        {
            RespawnManager.UpdateSpawnPoint(respawnPoint.transform.position);
            playerHere = true;
        }

        if (playerHere)
        {
            objectiveManager.ObjectiveTriggered = true;
            playerHere = false;
            this.gameObject.SetActive(false);
        }
    }
}
