/*
 * Author: Elisha
 * Description: This script holds all the information about the buttons that are in the pause menu such as the hover mechanic and clicking.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;
using TMPro;

public class PauseInformation : MonoBehaviour
{
    [Tooltip("Function to call when button is clicked")]
    [SerializeField]
    private UnityEvent m_onClickEvent;
    private TextMeshProUGUI m_text;
    private Color m_originalImage;
    int m_hoverers;

    private void Start()
    {
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        m_originalImage = m_text.color;
    }

    public void Click()
    {
        m_onClickEvent.Invoke();
    }

    public void Hover()
    {
        m_text.color = Color.gray;
    }

    public void Pressed()
    {
        m_text.color = Color.gray;
    }

    public void Unhover()
    {
        m_text.color = m_originalImage;
    }
}