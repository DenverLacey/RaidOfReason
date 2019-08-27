using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
/*
 * Author: Afridi Rahim, Denver Lacey

*/
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Room's Objective")]
    private BaseObjective m_objective;
    [Tooltip("Scene Index To Switch to")]
    public int sceneIndex;

    [Tooltip("Temporary Bool, will be removed")]
    public bool tempCleared;

    public Text objectiveTimer;
    public GameObject objectiveComplete;
    public GameObject objectiveFailed;

    private void Awake()
    {
        if (m_objective)
        {
            m_objective.Awake();
            objectiveComplete.SetActive(false);
            objectiveFailed.SetActive(false);
            objectiveTimer.text = m_objective.Timer().ToString("f0");
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
                StartCoroutine(TimeTillChange());
                SceneManager.LoadScene(sceneIndex);
            }
            else if (m_objective.HasFailed())
            {
                Debug.LogFormat("{0} has been failed", m_objective);
                StartCoroutine(TimeTillChange());
                SceneManager.LoadScene(0);
                // to fail stuff
            }

            if (m_objective.Timer() > 0)
            {
                objectiveTimer.text = m_objective.Timer().ToString("f0");
            }
            else
            {
                objectiveTimer.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator TimeTillChange()
    {
        yield return new WaitForSeconds(50.0f);
        if (m_objective.IsDone())
        {
            objectiveComplete.SetActive(true);
        }
        else if (m_objective.HasFailed())
        {
            objectiveFailed.SetActive(true);
        }
    }
}
