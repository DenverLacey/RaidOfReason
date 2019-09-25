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

    private int m_sceneIndex;
    private int m_prevSceneIndex;

    private bool m_titleScreenVisited = false;

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

    public static bool IsTitleScreenVisited()
    {
        return m_instance.m_titleScreenVisited;
    }
}
