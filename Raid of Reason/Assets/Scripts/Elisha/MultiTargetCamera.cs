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
	[SerializeField]
	[Tooltip("How much the camera will move towards its destination per lerp step")]
	private float m_lerpAmount = 0.5f;

	[SerializeField]
	[Tooltip("How much the camera will rotate towards its destination per lerp step")]
	private float m_rotLerpAmont = 0.01f;

	[SerializeField]
	[Tooltip("How close two players have to be to be considered a group")]
	private float m_maxGroupDistance = 50f;

	[SerializeField]
	[Tooltip("Chages Y offset based on cameras Y position")]
	private float m_yOffsetScalar = 0.025f;

	[SerializeField]
	[Tooltip("Minimum height of the camera")]
	private float m_minYPosition = 20f;

	[SerializeField]
	[Tooltip("Maximum height if the camera")]
	private float m_maxYPosition = 35f;

	[SerializeField]
	[Tooltip("Offset of the camera")]
	private Vector2 m_offset = new Vector3(0, -30);

	public float centerOffset = 10f;

	[SerializeField]
	[Min(0.001f)]
	private float m_paddiing = 0f;

	/// <summary>
	/// Calculates where the camera should go and lerps the camera to that position
	/// </summary>
	void FixedUpdate()
	{
		//List<BaseCharacter> activePlayers = GameManager.Instance.AlivePlayers;

		//if (activePlayers.Count == 0)
		//	return;

		//var activePlayerPositions = new List<Vector3>();
		//foreach (var target in activePlayers)
		//{
		//	activePlayerPositions.Add(target.transform.position);
		//}

		//var playerBounds = new Bounds(activePlayerPositions[0], Vector3.one * m_paddiing);
		//for (int i = 1; i < activePlayerPositions.Count; i++)
		//{
		//	playerBounds.Encapsulate(activePlayerPositions[i]);
		//}

		//Vector3 averagePosition = activePlayerPositions[0];

		//// bias average towards players close to each other
		//foreach (var target in activePlayerPositions)
		//{
		//	var closeTargets = new List<Vector3>();
		//	foreach (var other in activePlayerPositions)
		//	{
		//		if (other == target)
		//			continue;

		//		float sqrDistance = (target - other).sqrMagnitude;

		//		if (sqrDistance < m_maxGroupDistance * m_maxGroupDistance)
		//		{
		//			closeTargets.Add(other);
		//		}
		//	}

		//	if (closeTargets.Count != 0)
		//	{
		//		averagePosition = target;
		//		foreach (var close in closeTargets)
		//			averagePosition += close;
		//		averagePosition /= closeTargets.Count + 1;
		//	}
		//}

		//float desiredY = playerBounds.size.magnitude / m_maxGroupDistance;
		//desiredY = Mathf.Lerp(m_minYPosition, m_maxGroupDistance, desiredY);

		//desiredY = Mathf.Clamp(desiredY, m_minYPosition, m_maxYPosition);

		//Vector3 desiredPosition = new Vector3(
		//	averagePosition.x + m_offset.x,
		//	desiredY,
		//	averagePosition.z + (m_offset.y * desiredY * m_yOffsetScalar)
		//);

		//transform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpAmount);

		//Quaternion lookRotation = Quaternion.LookRotation((averagePosition - transform.position).normalized, Vector3.up);
		//lookRotation *= Quaternion.Euler(centerOffset, 0, 0);

		//lookRotation.eulerAngles = new Vector3(
		//	lookRotation.eulerAngles.x,
		//	0,
		//	0
		//);

		//transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, m_rotLerpAmont);

		// check if all three players are within max group distance
		var alivePlayers = GameManager.Instance.AlivePlayers;
		Bounds allPlayersBounds = new Bounds(alivePlayers[0].transform.position, Vector3.one * m_paddiing);
		for (int i = 1; i < alivePlayers.Count; i++)
		{
			allPlayersBounds.Encapsulate(alivePlayers[i].transform.position);
		}

		if (allPlayersBounds.size.sqrMagnitude <= m_maxGroupDistance * m_maxGroupDistance)
		{
			MoveAndRotateCameraByBounds(allPlayersBounds);
			return;
		}

		Bounds twoPlayerBounds = new Bounds();
		bool groupFound = false;
		float closestSqrDistance = float.MaxValue;

		for (int i = 0; i < alivePlayers.Count; i++)
		{
			var p1 = alivePlayers[i];
			var p2 = alivePlayers[(i + 1) % alivePlayers.Count];

			float sqrDistance = (p1.transform.position - p2.transform.position).sqrMagnitude;

			if (sqrDistance <= m_maxGroupDistance * m_maxGroupDistance && sqrDistance < closestSqrDistance)
			{
				closestSqrDistance = sqrDistance;
				twoPlayerBounds = new Bounds(p1.transform.position, Vector3.one * m_paddiing);
				twoPlayerBounds.Encapsulate(p2.transform.position);
				groupFound = true;
			}
		}

		if (groupFound)
		{
			MoveAndRotateCameraByBounds(twoPlayerBounds);
		}
		else
		{
			MoveAndRotateCameraByBounds(new Bounds(alivePlayers[0].transform.position, Vector3.one * m_paddiing));
		}
	}

	public void MoveAndRotateCameraByBounds(Bounds bounds)
	{
		float desiredY = bounds.size.magnitude / m_maxGroupDistance;
		desiredY = Mathf.Lerp(m_minYPosition, m_maxGroupDistance, desiredY);
		desiredY = Mathf.Clamp(desiredY, m_minYPosition, m_maxYPosition);

		Vector3 desiredPosition = new Vector3(
			bounds.center.x + m_offset.x,
			desiredY,
			bounds.center.z + (m_offset.y * desiredY * m_yOffsetScalar)
		);

		transform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpAmount);

		Quaternion lookRotation = Quaternion.LookRotation((bounds.center - transform.position).normalized, Vector3.up);
		lookRotation *= Quaternion.Euler(centerOffset, 0, 0);

		lookRotation.eulerAngles = new Vector3(
			lookRotation.eulerAngles.x,
			0f,
			0f
		);

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, m_rotLerpAmont);
	}
}
