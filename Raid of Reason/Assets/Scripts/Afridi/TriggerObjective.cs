using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager objectiveManager;
    private BarrierManager Barriers;
    private bool KenronPass;
    private bool NashornPass;
    private bool TheaPass;
    private void Awake()
    {
        objectiveManager = FindObjectOfType<ObjectiveManager>();
        Barriers = FindObjectOfType<BarrierManager>();
        KenronPass = false;
        NashornPass = false;
        TheaPass = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kenron")
        {
            KenronPass = true;
        }
        if (other.tag == "Nashorn")
        {
            NashornPass = true;
        }
        if (other.tag == "Thea")
        {
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
