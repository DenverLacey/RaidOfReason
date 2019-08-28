﻿using System.Collections;
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
    [Tooltip("How long the current scene will wait until it loads the next scene")]
    public float timeUntilSceneChange;

    [Tooltip("Temporary Bool, will be removed")]
    public bool tempCleared;

    public Text objectiveTimer;
    public Text objectiveDescription;
    public Text showTitle;
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
            objectiveDescription.text = m_objective.GrabDescription();
            showTitle.text = m_objective.GrabTitle();
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
                StartCoroutine(TimeTillChange());
            }
            else if (m_objective.HasFailed())
            {
                StartCoroutine(TimeTillChange());
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
        if (m_objective.IsDone())
        {
            tempCleared = true;
            Debug.LogFormat("{0} is complete", m_objective);
            objectiveComplete.SetActive(true);
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(sceneIndex);
        }

        if (m_objective.HasFailed())
        {
            Debug.LogFormat("{0} has been failed", m_objective);
            objectiveFailed.SetActive(true);
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
