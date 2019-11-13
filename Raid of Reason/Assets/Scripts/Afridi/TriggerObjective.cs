using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    private BarrierManager Barriers;
    private int m_playersInGame;
    public GameObject respawnPoint;

    private bool playerHere = false;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        Barriers = FindObjectOfType<BarrierManager>();
        m_playersInGame = GameManager.Instance.AlivePlayers.Count;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kenron" && Utility.IsPlayerAvailable(CharacterType.KENRON))
        {
            RespawnManager.UpdateSpawnPoint(respawnPoint.transform.position);
            playerHere = true;
        }
        if (other.tag == "Kreiger" && Utility.IsPlayerAvailable(CharacterType.KREIGER))
        {
            RespawnManager.UpdateSpawnPoint(respawnPoint.transform.position);
            playerHere = true;
        }
        if (other.tag == "Thea" && Utility.IsPlayerAvailable(CharacterType.THEA))
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
