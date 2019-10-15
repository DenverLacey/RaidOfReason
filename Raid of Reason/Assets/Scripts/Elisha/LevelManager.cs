/*
 * Author: Elisha, Denver
 * Description: This script managers all the level changes and makes sure the levels load in conjuction to what scene index they are in. 
 *              This class is a singleton thats static which makes the userbility easier for us in other scripts if needed.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static LevelManager m_instance = null;
    private static Animator animator;
    public static int m_levelToLoad;

    private int m_sceneIndex;
    private int m_prevSceneIndex;

    private bool m_titleScreenVisited = false;

    public void Start()
    {
        animator = FindObjectOfType<Animator>();
    }

    public static void LoadNextLevel()
    {
        m_instance.m_prevSceneIndex = m_instance.m_sceneIndex;
        m_instance.m_sceneIndex++;
        SceneManager.LoadScene(m_instance.m_sceneIndex, LoadSceneMode.Single);
    }

    public static void LoadLastLevel()
    {
        int temp = m_instance.m_prevSceneIndex;
        m_instance.m_prevSceneIndex = m_instance.m_sceneIndex;
        m_instance.m_sceneIndex = temp;
        SceneManager.LoadScene(m_instance.m_sceneIndex, LoadSceneMode.Single);
    }

    public static void LoadLevel(int levelIndex)
    {
        m_instance.m_prevSceneIndex = m_instance.m_sceneIndex;
        m_instance.m_sceneIndex = levelIndex;
        SceneManager.LoadScene(m_instance.m_sceneIndex, LoadSceneMode.Single);
    }

    public static void LoadLevelFromName(string levelName)
    {
        int levelIndex = SceneManager.GetSceneByName(levelName).buildIndex;
        LoadLevel(levelIndex);
    }

    public static void VisitTitleScreen()
    {
        m_instance.m_titleScreenVisited = true;
    }

    public static void HaveNotVisitTitleScreen()
    {
        m_instance.m_titleScreenVisited = false;
    }

    public static bool IsTitleScreenVisited()
    {
        return m_instance.m_titleScreenVisited;
    }

    public static void FadeToLevel(int levelIndex)
    {
        m_levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }
}