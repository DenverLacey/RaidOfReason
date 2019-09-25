using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using XboxCtrlrInput;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color textColorNoAlpha;
    public GameObject mainMenu;
    public GameObject titleScreen;

    public void Start()
    {
        text.GetComponent<TextMeshProUGUI>();

        textColorNoAlpha = text.color;
        textColorNoAlpha.a = 0.0f;

        text.DOColor(textColorNoAlpha, 1).SetLoops(-1, LoopType.Yoyo);

        if (!LevelManager.IsTitleScreenVisited())
        {
            mainMenu.SetActive(false);
            titleScreen.SetActive(true);
            LevelManager.VisitTitleScreen();
        }
        else
        {
            mainMenu.SetActive(true);
            titleScreen.SetActive(false);
        }
    }

    public void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.Any))
        {
            mainMenu.SetActive(true);
            titleScreen.SetActive(false);
        }
    }
}