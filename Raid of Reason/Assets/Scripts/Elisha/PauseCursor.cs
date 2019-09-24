using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;
using UnityEngine.UI;

public class PauseCursor : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How fast the cursor will move")]
    private float m_speed;

    [HideInInspector]
    public XboxController controller = XboxController.Any;
    private PlayerIndex m_playerIndex;
    private CanvasScaler m_canvas;
    private Vector3 m_inactivePosition;

    public void Start()
    {
        m_canvas = FindObjectOfType<CanvasScaler>();

        m_inactivePosition = transform.position;

        switch (controller)
        {
            case XboxController.First:
                m_playerIndex = PlayerIndex.One;
                break;
        }
    }

    public void Update()
    {
        // if no controller assigned
        if (controller == XboxController.Any)
        {
            return;
        }
        DoMovement();
        DoButtonInput();
    }

    /// <summary>
	/// moves cursor
	/// </summary>
	void DoMovement()
    {
        // get input
        float x = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float y = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        // scale input by speed and time
        x *= m_speed * Time.unscaledDeltaTime;
        y *= m_speed * Time.unscaledDeltaTime;

        transform.Translate(x, y, 0);

        // clamp to screen
        float widthExtents = m_canvas.referenceResolution.x * m_canvas.scaleFactor / 2f;
        float heightExtents = m_canvas.referenceResolution.y * m_canvas.scaleFactor / 2f;

        Vector3 desiredPosition = transform.localPosition;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -widthExtents, widthExtents);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, -heightExtents, heightExtents);

        transform.localPosition = desiredPosition;
    }

    void DoButtonInput()
    {
        int layerMask = Utility.GetIgnoreMask("Ignore Raycast");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 1, layerMask);
        Debug.DrawRay(this.transform.position, transform.forward, Color.green);
        PauseInformation pauseInfo = hit.collider.GetComponent<PauseInformation>();

        if (hit.collider)
        {
            pauseInfo.Hover();
            if (XCI.GetButtonDown(XboxButton.A, controller))
            {
                pauseInfo.Click();
                pauseInfo.Pressed();
            }
        }
        else if(!hit.collider)
        {
            pauseInfo.Unhover();
        }
        DebugTools.LogVariable("hit collider", hit.collider);
    }

    public void SetController(int index)
    {
        switch (index)
        {
            case 1:
                controller = XboxController.First;
                m_playerIndex = PlayerIndex.One;
                break;

            default:
                Debug.LogFormat("invalid index: {0} tried to be assigned to {1}", index, gameObject.name);
                break;
        }
    }
}
