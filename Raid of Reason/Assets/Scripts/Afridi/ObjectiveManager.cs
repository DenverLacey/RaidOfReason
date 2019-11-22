using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
 * Author: Afridi Rahim, Denver Lacey
 * Description: Handles all The Objectives and thier designated Loops
 * Last Edited: 15/11/2019
*/
public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Objectives should be added in order of progression")]
    public List<BaseObjective> objectives = new List<BaseObjective>();
    public List<TriggerObjective> triggerObjectives = new List<TriggerObjective>();
    public BaseObjective currentObjective;
    public TriggerObjective triggerObjective;
    public BarrierManager barriers;
    public bool ObjectiveCompleted;
    public bool ObjectiveTriggered = false;

    public TextMeshProUGUI objectiveTimer;
    public TextMeshProUGUI objectiveDescription;

    public GameObject objectiveComplete;
    public GameObject objectiveFailed;
    [Tooltip("Time till Objective Completion/Failed dissapears")]
    public float objectiveFade;
    private int index = 4;

    private bool m_Completed;
    private bool m_Failed;
    private bool m_haveObjective;
    private bool m_protect;

    private void Awake()
    {
        // If There are trigger opbjectives
        if (triggerObjectives.Count > 1)
        {
            // Set them up
            foreach (TriggerObjective obj in triggerObjectives)
            {
                obj.gameObject.SetActive(false);
            }
            triggerObjective = triggerObjectives[0];
            triggerObjective.gameObject.SetActive(true);
        }

        // Init
        m_protect = false;
        currentObjective = objectives[0];
        currentObjective.Init();
        objectiveTimer.gameObject.SetActive(false);
        ObjectiveCompleted = false;
    }

    private void Update()
    {
        // If the current objective exists and has been triggered
        if (currentObjective && ObjectiveTriggered == true)
        {
            if (currentObjective is ProtectionObjective)
            {
				if (m_protect == false)
					MusicManager.Transition(MusicType.LVL_2_PHASE_2);

				m_protect = true;
			}

            if (triggerObjectives.Count > 1)
            {
                // Disable triggered trigger
                triggerObjective.gameObject.SetActive(false);
            }  
      
            // Set Descriptions and Timer
            objectiveTimer.gameObject.SetActive(true);
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            objectiveDescription.gameObject.SetActive(true);


            // Stores Complete/Failed Bools
            m_Completed = currentObjective.Completed();
            m_Failed = currentObjective.Failed();
            objectiveDescription.text = currentObjective.GrabDescription();
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentObjective.Timer().ToString("f0");
         
            currentObjective.Update();

            // On Completion 
            if (m_Completed && !m_Failed)
            {
                StartCoroutine(ObjectiveLoop());
            }

            // On Failure
            else if (m_Failed && !m_Completed)
            {
                StartCoroutine(ObjectiveLoop());
            }
        }
    }

    /// <summary>
    /// This Loop Determines the Paths of Failure or Completion
    /// </summary>
    /// <returns>Time till next objective Starts</returns>
    IEnumerator ObjectiveLoop()
    {
        #region Objective Complete
        // objective succeeded
        if (m_Completed && !m_Failed)
        {
            ObjectiveCompleted = true;

            if (objectives.Count > 1)
            {
			    objectives.RemoveAt(0);
                currentObjective = objectives[0];
                currentObjective.Init();
            }
            if (triggerObjectives.Count > 1)
            {
                triggerObjectives.RemoveAt(0);
                triggerObjective = triggerObjectives[0];
                triggerObjective.gameObject.SetActive(true);
            }

            // Reset Trigger
            ObjectiveTriggered = false;
            
            // Turns off Timers and Descriptions
            objectiveTimer.gameObject.SetActive(false);
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);

            objectiveComplete.SetActive(true);

            #region Objective Descriptions
            // Replaces Old objective texts with new objective
            objectiveDescription.text = objectiveDescription.text.Replace(currentObjective.GrabDescription(), currentObjective.GrabDescription());
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = objectiveTimer.text.Replace(currentObjective.Timer().ToString("f0"), currentObjective.Timer().ToString("f0"));
            #endregion
          
            yield return new WaitForSeconds(objectiveFade);

            if (ObjectiveCompleted == true && barriers != null)
            {
                objectiveComplete.SetActive(false);
                barriers.ManageBarriers();
                // Reset Completion
                ObjectiveCompleted = false;
            }

            // If its the last objective roll credits
            if (currentObjective is ProtectionObjective && m_protect == true)
            {
                LevelManager.FadeLoadLevel(index); 
            }
        }
        #endregion

        #region Objective Failed
        // objective failed
        if (m_Failed && !m_Completed)
        {
            // Turn Everything Off
            objectiveTimer.gameObject.SetActive(false);
            objectiveTimer.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            objectiveDescription.gameObject.SetActive(false);

            //Reset Trigger
            ObjectiveTriggered = false;
            objectiveFailed.SetActive(true);
            yield return new WaitForSecondsRealtime(objectiveFade);
            // Reload Scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion
    }
}
