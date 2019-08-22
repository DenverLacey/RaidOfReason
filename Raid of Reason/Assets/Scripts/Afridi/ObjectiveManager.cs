using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim, Denver Lacey

*/
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Room's Objective")]
    private BaseObjective m_objective;

    [Tooltip("Temporary Bool, will be removed")]
    public bool tempCleared;

    private void Awake()
    {
        if (m_objective)
        {
            m_objective.Awake();
        }
        tempCleared = false;
    }

    private void Update()
    {
        if (m_objective)
        {
            m_objective.Update();

            if (m_objective.IsDone())
            {
                tempCleared = true;
                Debug.LogFormat("{0} is complete", m_objective);
                // move to next
            }
            else if (m_objective.HasFailed())
            {
                Debug.LogFormat("{0} has been failed", m_objective);
                // to fail stuff
            }
        }
    }
}
