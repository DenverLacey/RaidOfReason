using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerCursor : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How quickly the cursor will move")]
	private float m_speed;

	[HideInInspector]
	public XboxController controller = XboxController.Any;

	private Vector2 m_input;

	private void Update()
	{
		if (controller == XboxController.Any)
		{
			return;
		}

		// get input
		m_input.x = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		m_input.y = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		// scale it
		m_input *= m_speed * Time.deltaTime;

		// move cursor
		transform.Translate(m_input.x, m_input.y, 0);
	}


}
