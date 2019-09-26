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
	[Tooltip("Maximum time it will take for camera to reach new position")]
	private float m_lerpAmount = 0.5f;

	[SerializeField]
	[Tooltip("Lowest the camera can go")]
	private float m_minYPosition = 20f;

	[SerializeField]
	[Tooltip("Highest the camera go")]
	private float m_maxYPosition = 50f;

	[SerializeField]
	[Tooltip("How sensitive the camera is to the size of the bounds")]
	private float m_distanceScalar = 1f;

	[SerializeField]
	[Tooltip("Chages Y offset based on cameras Y position")]
	private float m_yOffsetScalar = 0.025f;

	[SerializeField]
	[Tooltip("Offset of the camera")]
	private Vector2 m_offset = new Vector3(0, -30);

	[SerializeField]
	[Min(0.001f)]
	private float m_paddiing = 0f;

	/// <summary>
	/// Calculates where the camera should go and lerps the camera to that position
	/// </summary>
	void FixedUpdate()
    {
		List<BaseCharacter> activePlayers = GameManager.Instance.Players;
		activePlayers.RemoveAll(p => !p || p.playerState == BaseCharacter.PlayerState.DEAD);

		if (activePlayers.Count == 0)
			return;

		var activePlayerPositions = new List<Vector3>();
		foreach (var target in activePlayers)
		{
			activePlayerPositions.Add(target.transform.position);
		}

		var playerBounds = new Bounds(activePlayerPositions[0], Vector3.one * m_paddiing);
		for (int i = 1; i < activePlayerPositions.Count; i++)
		{
			playerBounds.Encapsulate(activePlayerPositions[i]);
		}

		Vector3 averagePosition = playerBounds.center;
		float desiredY = Mathf.Max(playerBounds.size.magnitude * m_distanceScalar, m_minYPosition);
		desiredY = Mathf.Min(desiredY, m_maxYPosition);

		Vector3 desiredPosition = new Vector3(
			averagePosition.x + m_offset.x,
			desiredY,
			averagePosition.z + (m_offset.y * desiredY * m_yOffsetScalar)
		);

		transform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpAmount);
	}
}
