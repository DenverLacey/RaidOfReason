using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGizmo : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Colour of Gizmo")]
	private Color m_colour = Color.red;

	[SerializeField]
	[Range(0, 1)]
	[Tooltip("Alpha of gizmo's colour")]
	private float m_alpha = 0.2f;

	[SerializeField]
	[Tooltip("Collider to use as Gizmo")]
	private BoxCollider m_collider;

	private void OnEnable()
	{
		m_collider = GetComponent<BoxCollider>();
	}

	/// <summary>
	/// Draws Box Collider as cube gizmo
	/// </summary>
	private void OnDrawGizmos()
	{
		// draw fill colour cube
		Gizmos.color = m_colour * m_alpha;
		Gizmos.DrawCube(m_collider.transform.position, m_collider.size);

		// draw wireframe
		Gizmos.color = m_colour;
		Gizmos.DrawWireCube(m_collider.transform.position, m_collider.size);
	}
}
