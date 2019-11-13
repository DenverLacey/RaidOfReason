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
    public List<BaseObjective> m_objectives = new List<BaseObjective>();
    public List<TriggerObjective> triggerObjectives = new List<TriggerObjective>();
    public BaseObjective m_currentObjective;
    public TriggerObjective m_triggerObjective;
    public BarrierManager barriers;

    public bool ObjectiveCompleted;
    public bool ObjectiveTriggered = false;
    public int BuildIndex;

    public Text objectiveTimer;
    public Text objectiveDescription;

    public GameObject billBoards;
    public GameObject objectiveComplete;
    public GameObject objectiveFailed;

    private bool m_isDone;
    private bool m_hasFailed;

    private void Awake()
    {
        if (triggerObjectives.Count > 1)
        {
            foreach (TriggerObjective obj in triggerObjectives)
            {
                obj.gameObject.SetActive(false);
            }
            m_triggerObjective = triggerObjectives[0];
            m_triggerObjective.gameObject.SetActive(true);
        }

        objectiveTimer.gameObject.SetActive(false);
        billBoards.SetActive(false);
        m_currentObjective = m_objectives[0];
        m_currentObjective.Init();
        ObjectiveCompleted = false;
    }

    private void Update()
    {

        if (m_currentObjective && ObjectiveTriggered == true)
        {
            if (triggerObjectives.Count > 1)
            {
                m_triggerObjective.gameObject.SetActive(false);
            }
            billBoards.SetActive(true);
            objectiveTimer.gameObject.SetActive(true);
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            objectiveDescription.gameObject.SetActive(true);

            #region Current Objective Init
                m_isDone = m_currentObjective.IsDone();
                m_hasFailed = m_currentObjective.HasFailed();
                objectiveDescription.text = m_currentObjective.GrabDescription();
                objectiveTimer.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = m_currentObjective.Timer().ToString("f0");
                #endregion

            m_currentObjective.Update();

            if (m_isDone && !m_hasFailed)
            {
                StartCoroutine(ObjectiveLoop());
            }
            else if (m_hasFailed && !m_isDone)
            {
                StartCoroutine(ObjectiveLoop());
            }

            if (m_currentObjective.Timer() > 0)
            {
                objectiveTimer.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = m_currentObjective.Timer().ToString("f0");
            }
            else
            {
                billBoards.SetActive(false);
                objectiveTimer.gameObject.SetActive(false);
                objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ObjectiveLoop()
    {
		// objective succeeded
        if (m_isDone && !m_hasFailed)
        {
            ObjectiveCompleted = true;

            if (barriers != null)
            {
                barriers.ManageBarriers();
            }

            if (m_objectives.Count > 1)
            {
			    m_objectives.RemoveAt(0);
                m_currentObjective = m_objectives[0];
                m_currentObjective.Init();
            }

            if (triggerObjectives.Count > 1)
            {
                triggerObjectives.RemoveAt(0);
                m_triggerObjective = triggerObjectives[0];
                m_triggerObjective.gameObject.SetActive(true);
            }

            ObjectiveCompleted = false;

            // Reset Trigger
            ObjectiveTriggered = false;

            billBoards.SetActive(true);
            objectiveTimer.gameObject.SetActive(false);
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);

            objectiveComplete.SetActive(true);

            if (m_objectives.Count <= 0)
            {
                LevelManager.FadeLoadLevel(BuildIndex);
            }

            #region Objective Descriptions
            // Replaces Old objective texts with new objective
            objectiveDescription.text = objectiveDescription.text.Replace(m_currentObjective.GrabDescription(), m_currentObjective.GrabDescription());
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = objectiveTimer.text.Replace(m_currentObjective.Timer().ToString("f0"), m_currentObjective.Timer().ToString("f0"));
            #endregion

            yield return new WaitForSeconds(5);

            objectiveComplete.SetActive(false);
        }

		// objective failed
        if (m_hasFailed && !m_isDone)
        {
            billBoards.SetActive(true);
            objectiveTimer.gameObject.SetActive(false);
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);

            ObjectiveTriggered = false;
            objectiveFailed.SetActive(true);
            yield return new WaitForSecondsRealtime(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
