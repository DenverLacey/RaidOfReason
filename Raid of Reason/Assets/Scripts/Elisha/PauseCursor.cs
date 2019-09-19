using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;
using UnityEngine.UI;

public class PauseCursor : MonoBehaviour
{
    [HideInInspector]
    public XboxController controller = XboxController.Any;
    private PlayerIndex m_playerIndex;
    private Transform m_collidedTransform = null;
    private CanvasScaler m_canvas;
    private Vector3 m_inactivePosition;

    [SerializeField]
    [Tooltip("How fast the cursor will move")]
    private float m_speed;


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
        if (XCI.GetButtonDown(XboxButton.A, controller) && m_collidedTransform)
        {
            m_collidedTransform.GetComponent<PauseInformation>().Click();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_collidedTransform = collision.transform;

        var info = m_collidedTransform.GetComponent<PauseInformation>();

        if (info)
        {
            info.Hover();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var info = m_collidedTransform.GetComponent<PauseInformation>();

        if (info)
        {
            info.Unhover();
        }

        m_collidedTransform = null;
    }
}
