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
	[Tooltip("Closest the camera will get to players")]
	private float m_minDistance = 5f;

	[SerializeField]
	[Tooltip("Farthest the camera will get to players")]
	private float m_maxDistance = 10f;

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

	/// <summary>
	/// Physics update.
	/// </summary>
	void FixedUpdate()
    {
        targets.RemoveAll(target => !target);

        // If all players are dead from the array dont do anything.
        if (targets.Count == 0)
		{
			return;
		}

        var bounds = new Bounds(targets[0].position, Vector3.zero);

		// calculate centre point of bounds
		Vector3 centre = GetCentrePoint(ref bounds);

		// calculate required distance
		m_currentDistance = Mathf.SmoothDamp(m_currentDistance, bounds.size.magnitude * m_distanceScalar, ref m_zoomVelocity, m_smoothTime * Time.fixedDeltaTime);
		m_currentDistance = Mathf.Clamp(m_currentDistance, m_minDistance, m_maxDistance);

		// calculate offset needed to achieve distance
		Vector3 requiredOffset = m_offset - transform.forward * m_currentDistance;

		transform.position = Vector3.SmoothDamp(transform.position, centre + requiredOffset, ref m_moveVelocity, m_smoothTime * Time.fixedDeltaTime);

		// do rotation stuff
		if (m_rotate)
		{
			// calculate direciton and rotation
			Vector3 direction = (centre - transform.position).normalized;
			Quaternion rotation = Quaternion.LookRotation(direction);
			Quaternion slerpRotation = Quaternion.Slerp(transform.rotation, rotation, m_rotateSlerpT);

			// rotate
			transform.rotation = Quaternion.Euler(
				slerpRotation.eulerAngles.x,
				slerpRotation.eulerAngles.y,
				transform.rotation.eulerAngles.z
			);
		}
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
}
