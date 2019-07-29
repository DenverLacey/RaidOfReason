using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerHandler : MonoBehaviour
{
    public Kenron kenronController;
    public Nashorn nashornController;
    public Theá theaController;
    public BaseCharacter Player;

    [SerializeField]
    private Canvas m_KCanvas;
    [SerializeField]
    private Canvas m_NCanvas;
    [SerializeField]
    private Canvas m_TCanvas;

    private bool m_SeeCanvas;

    private void Awake()
    {
        kenronController = FindObjectOfType<Kenron>();
        nashornController = FindObjectOfType<Nashorn>();
        theaController = FindObjectOfType<Theá>();
    }

    private void Update()
    {
        if (XCI.GetButtonDown(XboxButton.A, kenronController.m_controller))
        {
            if (m_KCanvas)
            {
                m_SeeCanvas = !m_SeeCanvas;
                m_KCanvas.gameObject.SetActive(m_SeeCanvas);
            }
        }
        if (XCI.GetButtonDown(XboxButton.A, nashornController.m_controller))
        {
            if (m_NCanvas)
            {
                m_SeeCanvas = !m_SeeCanvas;
                m_NCanvas.gameObject.SetActive(m_SeeCanvas);
            }
        }
        if (XCI.GetButtonDown(XboxButton.A, theaController.m_controller))
        {
            if (m_TCanvas)
            {
                m_SeeCanvas = !m_SeeCanvas;
                m_TCanvas.gameObject.SetActive(m_SeeCanvas);
            }
        }
    }
}
