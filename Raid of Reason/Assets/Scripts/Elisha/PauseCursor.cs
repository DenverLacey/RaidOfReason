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

    private PauseInformation m_pauseMenuInfo;
    
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

    /// <summary>
    /// Raycaster for pause menu cursor.
    /// </summary>
    void DoButtonInput()
    {
        // Create ray cast from cursors forward position.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, 5);
        // Debug purposes.
        Debug.DrawRay(this.transform.position, transform.forward * 100, Color.green);

        // Checks if the ray cast has hit a pause menu button with the same tag.
        if (hit.collider && hit.collider.tag == "PauseMenuButton")
        {
            // Unhover previous thing
            if (m_pauseMenuInfo)
            {
                m_pauseMenuInfo.Unhover();
            }

            // Hovers over the button.
            m_pauseMenuInfo = hit.collider.GetComponent<PauseInformation>();
            m_pauseMenuInfo.Hover();

            // If user presses A while on the button
            if (XCI.GetButtonDown(XboxButton.A, controller))
            {
                // Do click.
                m_pauseMenuInfo.Click();
                m_pauseMenuInfo.Pressed();
            }
        }
        // Othewise user isnt on a button 
        else if (m_pauseMenuInfo)
        {
            // Unhover.
            m_pauseMenuInfo.Unhover();
            m_pauseMenuInfo = null;
        }
        // Debug checks if collider has been hit and what object it is hit.
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
