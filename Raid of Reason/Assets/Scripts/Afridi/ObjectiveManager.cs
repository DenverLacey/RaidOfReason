using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("King of the Hill objective")]
    private KOTHObjective m_KingObjective;

    private void Update()
    {
        if (m_KingObjective != null)
            m_KingObjective.Update();
    }
}
