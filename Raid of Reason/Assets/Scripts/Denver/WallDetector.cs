using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
	[SerializeField]
	private ProjectileMove m_projectile;

	private void OnTriggerEnter(Collider other)
	{
		bool active = m_projectile.gameObject.activeSelf;

		if (other.gameObject.layer == LayerMask.NameToLayer("Environment"))
		{
			m_projectile.Destroy();
		}
	}
}
