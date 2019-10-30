/*
 * Author: Elisha
 * Description: This script allows the text mesh pro object specifically blink back and fourth.
 *              Also handles any controller start input to continue into the main menu.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using XboxCtrlrInput;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreenMenu : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera1;
    public CinemachineVirtualCamera VirtualCamera2;
    public Text text;
    public Color textColorNoAlpha;
    public GameObject mainMenu;
    public GameObject titleScreen;
    public GameObject m_startButton;

    public void Start()
    {
        text.GetComponent<Text>();
        textColorNoAlpha = text.color;
        textColorNoAlpha.a = 0.0f;
        text.DOColor(textColorNoAlpha, 1).SetLoops(-1, LoopType.Yoyo);
        VirtualCamera1.gameObject.SetActive(true);
        VirtualCamera2.gameObject.SetActive(false);
        m_startButton.SetActive(false);
    }
    public void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.Any))
        {
            VirtualCamera2.gameObject.SetActive(true);
            mainMenu.SetActive(true);
            m_startButton.SetActive(true);
        }
    }
}