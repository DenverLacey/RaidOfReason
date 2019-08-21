using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim, Denver Lacey

*/
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("King of the Hill objective")]
    private KOTHObjective m_KingObjective;

    public bool tempCleared;

    private void Update()
    {
        if (m_KingObjective != null)
        {
            m_KingObjective.Update();
        }
    }
}
