using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class TipManager : MonoBehaviour
{
    public List<GameObject> Tips = new List<GameObject>();
    private bool isTriggered = false;
    private float waitTime = 5f;

    private void Awake()
    {
        if (Tips.Count > 0)
        {
            foreach (GameObject tip in Tips)
            {
                tip.gameObject.SetActive(false);
            }
            Tips[0].SetActive(true);
        }
    }

    void Update()
    {
        if (Tips.Count > 0)
        {
            waitTime -= Time.deltaTime;
            if (waitTime < 0)
            {
                Time.timeScale = 0.0f;
            }
            if (XCI.GetButtonDown(XboxButton.A, XboxController.Any) && !isTriggered)
            {
                if (Tips[0] != null && !isTriggered)
                {
                    Tips[0].SetActive(false);
                    Tips.RemoveAt(0);

                    if (Tips.Count > 0)
                    {
                        Tips[0].SetActive(true);
                    }
                    if (Tips.Count == 0)
                    {
                        Time.timeScale = 1f;
                    }

                    isTriggered = true;
                }
            }
            else
            {
                isTriggered = false;
            }      
        }
    }
}
