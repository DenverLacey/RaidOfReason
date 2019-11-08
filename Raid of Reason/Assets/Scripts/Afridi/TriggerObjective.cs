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

    private bool KenronPass;
    private bool NashornPass;
    private bool TheaPass;

    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        Barriers = FindObjectOfType<BarrierManager>();
        m_playersInGame = GameManager.Instance.AlivePlayers.Count;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        KenronPass = false;
        NashornPass = false;
        TheaPass = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kenron")
        {
            RespawnManager.UpdateSpawnPoint(respawnPoint.transform.position);
            KenronPass = true;
        }
        if (other.tag == "Kreiger")
        {
            RespawnManager.UpdateSpawnPoint(respawnPoint.transform.position);
            NashornPass = true;
        }
        if (other.tag == "Thea")
        {
            RespawnManager.UpdateSpawnPoint(respawnPoint.transform.position);
            TheaPass = true;
        }

        if (KenronPass && NashornPass && TheaPass)
        {
            objectiveManager.ObjectiveTriggered = true;
            KenronPass = false;
            NashornPass = false;
            TheaPass = false;
            this.gameObject.SetActive(false);
        }    
    }
}
