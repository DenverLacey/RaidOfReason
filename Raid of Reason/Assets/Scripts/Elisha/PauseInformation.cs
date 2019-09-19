using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.Events;

public class PauseInformation : MonoBehaviour
{
    [Tooltip("Function to call when button is clicked")]
    [SerializeField]
    UnityEvent m_onClickEvent;
    private Image m_images;
    private Color m_originalImage;
    int m_hoverers;

    private void Start()
    {
        m_images = GetComponent<Image>();
    }

    public void Click()
    {
        m_onClickEvent.Invoke();
    }

    public void Hover()
    {
        Interlocked.Increment(ref m_hoverers);

        if (m_hoverers <= 0)
        {
            m_hoverers = 0;
            m_images.color *= Color.black;
        }
    }

    public void Unhover()
    {
        Interlocked.Increment(ref m_hoverers);
        m_images.color = m_originalImage;
    }
}