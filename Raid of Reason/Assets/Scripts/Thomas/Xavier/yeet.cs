using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using UnityEngine.SceneManagement;

public class yeet : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Back, XboxController.Any))
        {
            SceneManager.LoadScene(0);
        }
    }
}
