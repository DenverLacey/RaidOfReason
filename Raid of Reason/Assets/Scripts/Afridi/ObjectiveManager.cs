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
    [Tooltip("Objectives should be added in order of progression")]
    private List<BaseObjective> m_objective = new List<BaseObjective>();
    private List<BaseObjective> m_currentObjective = new List<BaseObjective>();

    public bool ObjectiveCompleted;
    public bool ObjectiveTriggered;

    public Text objectiveTimer;
    public Text objectiveDescription;
    public Text showTitle;

    public GameObject objectiveComplete;
    public GameObject objectiveFailed;
    public GameObject skillTreeUnlock;

    private bool m_isDone;
    private bool m_hasFailed;

    private void Awake()
    {
        m_currentObjective.Add(m_objective[0]);
        if (m_currentObjective[0])
        {
            #region Objective Init
            m_currentObjective[0].Awake();
            objectiveComplete.SetActive(false);
            objectiveFailed.SetActive(false);
            skillTreeUnlock.SetActive(false);
            objectiveTimer.gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);
            showTitle.gameObject.SetActive(false);
            #endregion
        }
        ObjectiveCompleted = false;
        ObjectiveTriggered = false;
    }

    private void Update()
    {
        if (m_currentObjective[0] && ObjectiveTriggered == true)
        {
            objectiveTimer.gameObject.SetActive(true);
            objectiveDescription.gameObject.SetActive(true);
            showTitle.gameObject.SetActive(true);

            #region Current Objective Init
                m_isDone = m_currentObjective[0].IsDone();
                m_hasFailed = m_currentObjective[0].HasFailed();
                objectiveDescription.text = m_currentObjective[0].GrabDescription();
                showTitle.text = m_currentObjective[0].GrabTitle();
                objectiveTimer.text = m_currentObjective[0].Timer().ToString("f0");
                #endregion

            m_currentObjective[0].Update();

            if (m_isDone && !m_hasFailed)
            {
                StartCoroutine(ObjectiveLoop());
            }
            else if (m_hasFailed && !m_isDone)
            {
                StartCoroutine(ObjectiveLoop());
            }

            if (m_currentObjective[0].Timer() > 0)
            {
                objectiveTimer.text = m_currentObjective[0].Timer().ToString("f0");
            }
            else
            {
                objectiveTimer.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ObjectiveLoop()
    {
        if (m_isDone && !m_hasFailed)
        {
            m_objective.Remove(m_objective[0]);
            m_currentObjective.Remove(m_currentObjective[0]);

            ObjectiveCompleted = true;
            // Reset Trigger
            ObjectiveTriggered = false;

            objectiveTimer.gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);
            showTitle.gameObject.SetActive(false);

            objectiveComplete.SetActive(true);

            yield return new WaitForSecondsRealtime(5);

            m_objective.Add(m_currentObjective[0]);

            #region Objective Descriptions
            // Replaces Old objective texts with new objective
            objectiveDescription.text = objectiveDescription.text.Replace(m_currentObjective[0].GrabDescription(), m_currentObjective[0].GrabDescription());
            showTitle.text = showTitle.text.Replace(m_currentObjective[0].GrabTitle(), m_currentObjective[0].GrabTitle());
            objectiveTimer.text = objectiveTimer.text.Replace(m_currentObjective[0].Timer().ToString("f0"), m_currentObjective[0].Timer().ToString("f0"));
            #endregion

            objectiveComplete.SetActive(false);
        }
        if (m_hasFailed && !m_isDone)
        {
            objectiveTimer.gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);
            showTitle.gameObject.SetActive(false);
            ObjectiveTriggered = false;
            objectiveFailed.SetActive(true);
            yield return new WaitForSecondsRealtime(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
