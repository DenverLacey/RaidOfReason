/*
 * Author: Denver
 * Description:	Handles functionality for when a thea projectile hits a wall
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys Thea Projectile when it hits the environment
/// </summary>
public class WallDetector : MonoBehaviour
{
	[SerializeField]
	private ProjectileMove m_projectile;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
		{
			m_projectile.Destroy();
		}
	}
}
