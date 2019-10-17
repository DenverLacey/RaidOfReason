/*
 * Author: Denver
 * Description:	Handles all functionality for Offscreen Player Tracker Effect
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls UI For the Offscreen Player Tracker Effect
/// </summary>
public class OffscreenCameraController : MonoBehaviour
{
	[Tooltip("Transform of the object to follow")]
	[SerializeField]
	private Transform m_target;

	[Tooltip("Camera to use to follow target")]
	[SerializeField]
	private Transform m_camera;

	[Tooltip("Threshold for how much target must be offscreen before activated offscreen camera effect")]
	[Range(0f, 1f)]
	[SerializeField]
	private float m_threshold = 0.01f;

	[Tooltip("How far in the indicator will be from the edge of the screen")]
	[SerializeField]
	private float m_margin;

	[Tooltip("How quickly the indicator will move across the screen")]
	[SerializeField]
	private float m_lerpAmount = 0.1f;

	private Transform m_activator;
	private Transform m_directionIndicator;

	private Camera m_mainCamera;

	private Vector3 m_offset;

    // Start is called before the first frame update
    void Start()
    {
		// get child objects
		m_activator = transform.Find("Activator");
		m_directionIndicator = transform.FindChildByRecursive("DirectionIndicator");
       
		m_activator.gameObject.SetActive(false);

		// get main camera
		m_mainCamera = FindObjectOfType<MultiTargetCamera>().GetComponent<Camera>();

		m_offset = m_camera.position - m_target.position;
    }

    // Update is called once per frame
    void Update()
    {
		// if target object is turned off, turn off UI
		if (!m_target || !m_target.gameObject.activeSelf)
		{
			m_activator.gameObject.SetActive(false);
			return;
		}

		Vector3 viewportPosition = m_mainCamera.WorldToViewportPoint(m_target.position);
		viewportPosition *= viewportPosition.z > 0 ? 1 : -1;

		// if on screen
		if (viewportPosition.x > -m_threshold && viewportPosition.x < 1 + m_threshold && viewportPosition.y > -m_threshold && viewportPosition.y < 1 + m_threshold)
		{
			m_activator.gameObject.SetActive(false);
		}
		else
		{
			m_activator.gameObject.SetActive(true);

			m_camera.position = m_target.position + m_offset;

			// calculate required rotation for direction indicator
			Vector3 mid = new Vector3(.5f, .5f);
			Vector3 direction = (mid - viewportPosition).normalized;
			direction.x *= -1f;
			direction.z = 0f;
			float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
			m_directionIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			// move image to edge of screen
			Vector3 position = transform.position;

			if (AngleIsInBetween(angle, 45, -45))
			{
				position.y = 0;
				position.x = Mathf.InverseLerp(-45, 45, angle);
			}
			else if (AngleIsInBetween(angle, -45, -135))
			{
				position.x = 0;
				position.y = Mathf.InverseLerp(-45, -135, angle);
			}
			else if (AngleIsInBetween(angle, -135, 135))
			{
				position.y = 1f;

				if (angle > -180 && angle < 135)
				{
					position.x = Mathf.InverseLerp(-135, -180, angle) / 2f;
				}
				else 
				{
					position.x = 1 - Mathf.InverseLerp(135, 180, angle) / 2f;
				}
			}
			else
			{
				position.x = 1;
				position.y = Mathf.InverseLerp(45, 135, angle);
			}

			position.x = Mathf.Clamp(position.x, m_margin, 1 - m_margin) * Screen.width;
			position.y = Mathf.Clamp(position.y, m_margin, 1 - m_margin) * Screen.height;

			transform.position = position;
		}
    }

	/// <summary>
	/// Determines if an angle is in between two other angles
	/// </summary>
	/// <param name="value">
	/// The angle that may be in between two angles
	/// </param>
	/// <param name="a">
	/// The first angle
	/// </param>
	/// <param name="b">
	/// The second angle
	/// </param>
	/// <returns>
	/// True if value is in between angles a and b. False if otherwise
	/// </returns>
	private bool AngleIsInBetween(float value, float a, float b)
	{
		float rAngle = ((b - a) % 360 + 360) % 360;
		if (rAngle >= 180)
		{
			float tAngle = a;
			a = b;
			b = tAngle;
		}

		if (a <= b)
			return value >= a && value <= b;
		else
			return value >= a || value <= b;
	}
}
