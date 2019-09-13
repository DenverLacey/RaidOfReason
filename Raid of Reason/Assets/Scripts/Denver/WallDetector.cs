using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
	[SerializeField]
	private ProjectileMove m_projectile;

	private bool CollidedWithWall(Collider other)
	{
		return !Utility.TagIsPlayerTag(other.tag) && other.tag != "Enemy" && other.tag != "EnemyManager" && other.name != m_projectile.name && other.name != name && other.name != "EnemyProjectile(Clone)";
	}

	private void OnTriggerEnter(Collider other)
	{
		bool active = m_projectile.gameObject.activeSelf;

		if (CollidedWithWall(other))
		{
			m_projectile.Destroy();
		}
	}
}
