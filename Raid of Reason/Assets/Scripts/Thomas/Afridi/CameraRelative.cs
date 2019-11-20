using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;

public class CameraRelative : MonoBehaviour
{
    public BaseCharacter player;
    private MultiTargetCamera m_camera;

    private void Start()
    {
        m_camera = FindObjectOfType<MultiTargetCamera>();
    }
    // Update is called once per frame
    void Update()
    {
        // get stick input
        float leftX = XCI.GetAxis(XboxAxis.LeftStickX, player.controller);
        float leftY = XCI.GetAxis(XboxAxis.LeftStickY, player.controller);

        float rightX = XCI.GetAxis(XboxAxis.RightStickX, player.controller);
        float rightY = XCI.GetAxis(XboxAxis.RightStickY, player.controller);

        // make sure player movement is relative to the direction of the cameras forward
        Vector3 input = new Vector3(leftX, 0, leftY);

        // make sure player direction override is relative to the direction of the cameras forward
        Vector3 directionOverride = new Vector3(rightX, 0, rightY);

        // make input vectors relative to camera's rotation
        Vector3 camRotEuler = m_camera.transform.eulerAngles;
        camRotEuler.x = 0.0f; camRotEuler.z = 0.0f;

        input = Quaternion.AngleAxis(camRotEuler.y, Vector3.up) * input;
        directionOverride = Quaternion.AngleAxis(camRotEuler.y, Vector3.up) * directionOverride;
    }
}