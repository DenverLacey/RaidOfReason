/* 
 * Author: Elisha, Denver
 * Description: Handles how the camera operates within the game by making sure our main characters are all within the screen 
 *              at once, by adjusting its zoom and position based on how many players there are.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera that automatically orients itself so all targets can be seen
/// </summary>
[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField]
	[Tooltip("Maximum time it will take for camera to reach new position")]
	private float m_smoothTime = 20f;

	[SerializeField]
	[Tooltip("Lowest the camera can go")]
	private float m_minYPosition = 5f;

	[SerializeField]
	[Tooltip("Highest the camera can go")]
	private float m_maxYPosition = 10f;

	[SerializeField]
	[Tooltip("How sensitive the camera is to the size of the bounds")]
	private float m_distanceScalar = 0.6f;

	[SerializeField]
	[Tooltip("Offset of the camera")]
	private Vector3 m_offset;

	[Tooltip("Every object the camera will attempt to encapsulate")]
	public List<Transform> targets;

	[Header("Rotation")]
	[SerializeField]
	[Tooltip("If camera should rotate to look at centre")]
	private bool m_rotate = true;

	[SerializeField]
	[HideInInspector]
	[Tooltip("t value for Quaternion.Slerp calculation")]
	private float m_rotateSlerpT = 0.025f;

	private Vector3 m_moveVelocity;
	private float m_zoomVelocity;
	private float m_currentDistance;

	private Vector3 m_averagePosition = new Vector3();

	private float m_middleDistance;

	private void Start()
	{
		m_middleDistance = (m_minYPosition + m_maxYPosition) / 2f;
	}

	/// <summary>
	/// Physics update.
	/// </summary>
	void FixedUpdate()
    {
        // If all players are dead from the array dont do anything.
        if (targets.Count == 0)
		{
			return;
		}

		DoMovement();
		DoDolly();
    }

    /// <summary>
    /// This function allows the camera to adjust to the centre point based on where the players are.
    /// </summary>
    /// <param name="bounds"> Bounds of all targets </param>
    /// <returns> Vector 3 value. </returns>
    Vector3 GetCentrePoint(ref Bounds bounds) {

        // Checks if theres only 1 player in the array of targets.
        if (targets.Count == 1) 
		{
            // If so then return the position of the target left.
            return targets[0].position;
        }

        // For every transform in the array of targets.
        foreach (Transform t in targets) 
		{
            // Grow the bounds of that targets position.
            bounds.Encapsulate(t.position);
        }

        return bounds.center;
    }

	void DoMovement()
	{
		foreach (var target in targets)
		{
			m_averagePosition += target.position;
		}
		m_averagePosition /= targets.Count;
		transform.position = Vector3.SmoothDamp(transform.position, m_averagePosition + m_offset, ref m_moveVelocity, m_smoothTime);
	}

	void DoDolly()
	{
		Bounds playerBounds = new Bounds();
		foreach (var target in targets)
		{
			playerBounds.Encapsulate(target.position);
		}
	}
}
