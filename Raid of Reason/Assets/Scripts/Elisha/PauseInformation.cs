using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;

public class PauseInformation : MonoBehaviour
{
    [Tooltip("Function to call when button is clicked")]
    [SerializeField]
    private UnityEvent m_onClickEvent;
    private Image m_images;
    private Color m_originalImage;
    int m_hoverers;

    private void Start()
    {
        m_images = GetComponentInChildren<Image>();
        m_originalImage = m_images.color;
    }

    public void Click()
    {
        m_onClickEvent.Invoke();
    }

    public void Hover()
    {
        m_images.color = Color.gray;
    }

    public void Pressed()
    {
        m_images.color = Color.black;
    }

    public void Unhover()
    {
        m_images.color = m_originalImage;
    }
}