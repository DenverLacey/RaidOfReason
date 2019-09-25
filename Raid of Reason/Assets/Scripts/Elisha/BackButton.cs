using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : InteractableUIElement
{
    [SerializeField]
    [Tooltip("Start Button Object")]
    private GameObject m_backButton;

    [SerializeField]
    [Tooltip("Scene Index of level")]
    private int m_LevelIndex;

    public void OnPressed()
    {
        LevelManager.LoadLastLevel();
    }
}
