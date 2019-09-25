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
	private float m_lerpAmount = 0.1f;

	[SerializeField]
	[Tooltip("Lowest the camera can go")]
	private float m_minYPosition = 20f;

	[SerializeField]
	[Tooltip("How sensitive the camera is to the size of the bounds")]
	private float m_distanceScalar = 1.2f;

	[SerializeField]
	[Tooltip("Chages Y offset based on cameras Y position")]
	private float m_yOffsetScalar = 0.025f;

	[SerializeField]
	[Tooltip("Offset of the camera")]
	private Vector2 m_offset;

	[SerializeField]
	[Min(0.001f)]
	private float m_paddiing = 4f;

	[Tooltip("Every object the camera will attempt to encapsulate")]
	public List<BaseCharacter> targets;
	private float m_zoomVelocity;

	/// <summary>
	/// Physics update.
	/// </summary>
	void LateUpdate()
    {
        // If all players are dead from the array dont do anything.
        if (targets.Count == 0 || targets.TrueForAll(p => p.playerState == BaseCharacter.PlayerState.DEAD))
		{
			return;
		}

		List<Vector3> activePlayerPositions = new List<Vector3>();
		foreach (var target in targets)
		{
			if (target.playerState == BaseCharacter.PlayerState.DEAD)
				continue;

			activePlayerPositions.Add(target.transform.position);
		}

		Bounds playerBounds = new Bounds(activePlayerPositions[0], Vector3.one * m_paddiing);
		for (int i = 1; i < activePlayerPositions.Count; i++)
		{
			playerBounds.Encapsulate(activePlayerPositions[i]);
		}

		Vector3 averagePosition = playerBounds.center;
		float desiredY = Mathf.Max(playerBounds.size.magnitude * m_distanceScalar, m_minYPosition);

		Vector3 desiredPosition = new Vector3(
			averagePosition.x + m_offset.x,
			desiredY,
			averagePosition.z + (m_offset.y * desiredY * m_yOffsetScalar)
		);

		transform.position = Vector3.Lerp(transform.position, desiredPosition, m_lerpAmount);
	}
}
