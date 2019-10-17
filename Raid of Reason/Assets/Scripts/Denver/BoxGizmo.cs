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

	[SerializeField]
	[Range(0, 1)]
	[Tooltip("Alpha of gizmo's colour")]
	private float m_alpha = 0.2f;

	[SerializeField]
	[Tooltip("Colliders to use as Gizmo")]
	private BoxCollider[] m_colliders;

	/// <summary>
	/// Draws Box Collider as cube gizmo
	/// </summary>
	private void OnDrawGizmos()
	{
		if (!m_drawGizmo)
			return;

		foreach (var collider in m_colliders)
		{
			if (!collider || !collider.enabled)
				continue;

			// draw fill colour cube
			Gizmos.color = m_colour * m_alpha;
			Gizmos.DrawCube(transform.position + collider.center, collider.size);

			// draw wireframe
			Gizmos.color = m_colour;
			Gizmos.DrawWireCube(transform.position + collider.center, collider.size);
		}
	}
}
