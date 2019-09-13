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

		if (!Utility.TagIsPlayerTag(other.tag) && other.tag != "Enemy" && other.tag != "EnemyManager" && other.name != m_projectile.name)
		{
			m_projectile.Destroy();
		}
	}
}
