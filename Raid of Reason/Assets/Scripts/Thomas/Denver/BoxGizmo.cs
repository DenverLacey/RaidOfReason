/*
 * Author: Denver
 * Description:	Makes a box gizmo for an object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes a box gizmo for an object
/// </summary>
public class BoxGizmo : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Toggle Gizmo on and off")]
	private bool m_drawGizmo = true;

	[SerializeField]
	[Tooltip("Colour of Gizmo")]
	private Color m_colour = Color.red;

	private BoxCollider[] m_colliders;
	private bool m_collidersFound = false;

	/// <summary>
	/// Draws Box Collider as cube gizmo
	/// </summary>
	private void OnDrawGizmos()
	{
		if (!m_drawGizmo)
			return;

		if (!m_collidersFound)
		{
			m_colliders = GetComponents<BoxCollider>();
			m_collidersFound = true;
		}

		foreach (var collider in m_colliders)
		{
			if (!collider || !collider.enabled)
				continue;

			Gizmos.color = m_colour;

			// calculate min and max
			Vector3 min = transform.position - collider.size / 2f;
			Vector3 max = transform.position + collider.size / 2f;

			// bottom face
			Vector3 backBottomLeft = min;
			Vector3 frontBottomLeft = new Vector3(min.x, min.y, max.z);
			Vector3 frontBottonRight = new Vector3(max.x, min.y, max.z);
			Vector3 backBottomRight = new Vector3(max.x, min.y, min.z);

			// top face
			Vector3 backTopLeft = new Vector3(min.x, max.y, min.z);
			Vector3 frontTopLeft = new Vector3(min.x, max.y, max.z);
			Vector3 frontTopRight = max;
			Vector3 backTopRight = new Vector3(max.x, max.y, min.z);

			// rotate faces
			backBottomLeft		= transform.rotation * backBottomLeft;
			frontBottomLeft		= transform.rotation * frontBottomLeft;
			frontBottonRight	= transform.rotation * frontBottonRight;
			backBottomRight		= transform.rotation * backBottomRight;
			backTopLeft			= transform.rotation * backTopLeft;
			frontTopLeft		= transform.rotation * frontTopLeft;
			frontTopRight		= transform.rotation * frontTopRight;
			backTopRight		= transform.rotation * backTopRight;

			// draw bottom lines
			Gizmos.DrawLine(backBottomLeft, frontBottomLeft);
			Gizmos.DrawLine(frontBottomLeft, frontBottonRight);
			Gizmos.DrawLine(frontBottonRight, backBottomRight);
			Gizmos.DrawLine(backBottomRight, backBottomLeft);

			// draw top lines
			Gizmos.DrawLine(backTopLeft, frontTopLeft);
			Gizmos.DrawLine(frontTopLeft, frontTopRight);
			Gizmos.DrawLine(frontTopRight, backTopRight);
			Gizmos.DrawLine(backTopRight, backTopLeft);

			// connector lines
			Gizmos.DrawLine(backBottomLeft, backTopLeft);
			Gizmos.DrawLine(frontBottomLeft, frontTopLeft);
			Gizmos.DrawLine(frontBottonRight, frontTopRight);
			Gizmos.DrawLine(backBottomRight, backTopRight);
		}
	}
}
