﻿/*
 * Author: Elisha, Denver
 * Description: This script Managers all the level changes and makes sure the levels load in conjuction to what scene index they are in. 
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
        // Singleton
        if (m_instance == null)
        {
            m_instance = this;
            // this instance will never be destroyed when scenes are changing
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static LevelManager m_instance = null;
    private static Animator animator;

    private int m_sceneIndex;
	public int SceneIndex { get => m_sceneIndex; }

    private int m_prevSceneIndex;
	public int PrevSceneIndex { get => m_prevSceneIndex; }

    private int m_sceneToLoad;

    private bool m_titleScreenVisited = false;
    public static bool TitleScreenVisited { get => m_instance.m_titleScreenVisited; set => m_instance.m_titleScreenVisited = true; }

    public void Start()
    {
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += FadeFromBlack;
    }

    /// <summary>
    /// Loads the next level within the index.
    /// </summary>
    public static void LoadNextLevel()
    {
        m_instance.m_prevSceneIndex = m_instance.m_sceneIndex;
        m_instance.m_sceneIndex++;
        SceneManager.LoadScene(m_instance.m_sceneIndex, LoadSceneMode.Single);
    }

    /// <summary>
    /// The API that fades to the next index in the build settings.
    /// </summary>
    public static void FadeLoadNextLevel()
    {
        m_instance.m_sceneIndex++;
        FadeToLevel(m_instance.m_sceneIndex);
    }

    /// <summary>
    /// Loads the last level within the build settings.
    /// </summary>
    public static void LoadLastLevel()
    {
        int temp = m_instance.m_prevSceneIndex;
        m_instance.m_prevSceneIndex = m_instance.m_sceneIndex;
        m_instance.m_sceneIndex = temp;
        SceneManager.LoadScene(m_instance.m_sceneIndex, LoadSceneMode.Single);
    }

    /// <summary>
    /// Fade loads the last level in the index.
    /// </summary>
    public static void FadeLoadLastLevel()
    {
        FadeToLevel(m_instance.m_prevSceneIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelIndex"></param>
    public static void LoadLevel(int levelIndex)
    {
        m_instance.m_prevSceneIndex = m_instance.m_sceneIndex;
        m_instance.m_sceneIndex = levelIndex;
        SceneManager.LoadScene(m_instance.m_sceneIndex, LoadSceneMode.Single);
    }

    /// <summary>
    /// The API that calls the instance to fade to level.
    /// </summary>
    /// <param name="levelIndex"></param>
    public static void FadeLoadLevel(int levelIndex)
    {
        FadeToLevel(levelIndex);
    }

    /// <summary>
    /// Loads the level by name that is set in the parameter.
    /// </summary>
    /// <param name="levelName"></param>
    public static void LoadLevelFromName(string levelName)
    {
        int levelIndex = SceneManager.GetSceneByName(levelName).buildIndex;
        LoadLevel(levelIndex);
    }

    public static void VisitTitleScreen()
    {
        m_instance.m_titleScreenVisited = true;
    }

    /// <summary>
    /// The instance of fade to level.
    /// </summary>
    /// <param name="levelIndex"></param>
    private static void FadeToLevel(int levelIndex)
    {
        m_instance.m_sceneToLoad = levelIndex;
        animator.SetBool("FadeOut", true);
    }

    /// <summary>
    /// This function is for the 'Fade_Out' animation event in which this calls in the inspector.
    /// Applied it like this so it knows when to fade out in the next scene.
    /// </summary>
    public void FadeComplete()
    {
        LoadLevel(m_sceneToLoad);
    }

    /// <summary>
    /// This function gets called when a new scene gets loaded.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="loadSceneMode"></param>
    private void FadeFromBlack(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (animator != null)
        animator.SetBool("FadeOut", false);
    }
}