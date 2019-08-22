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

    [SerializeField]
    [Tooltip("King of the Hill objective")]
    private CountdownObjective m_CountDownObjective;

    [Tooltip("Temporary Bool, will be removed")]
    public bool tempCleared;

    private void Awake()
    {
        m_CountDownObjective.Awake();
        tempCleared = false;
    }

    private void Update()
    {
        if (m_KingObjective != null)
        {
            m_KingObjective.Update();

            if (m_KingObjective.IsDone() == true)
            {
                tempCleared = true;
                // Move to next
            }
        }
        if (m_CountDownObjective != null)
        {
            m_CountDownObjective.Update();

            if (m_CountDownObjective.IsDone() == true)
            {
                tempCleared = true;
                Debug.Log("Well Done");
                // Move to next
            }
        }
    }
}
