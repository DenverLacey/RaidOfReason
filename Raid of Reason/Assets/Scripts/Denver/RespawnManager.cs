using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	[Tooltip("Respawn Delay")]
	[SerializeField]
	private float m_respawnDelay = 5f;

	private static RespawnManager ms_instance;

	private static Dictionary<BaseCharacter, Vector3> m_spawnPoints;

	private void Awake()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public static void UpdateSpawnPoint(Vector3 spawnPoint)
	{
		Dictionary<BaseCharacter, Vector3> spawnPoints = new Dictionary<BaseCharacter, Vector3>();
		foreach (var pair in m_spawnPoints)
		{
			Vector3 relativePos = spawnPoint;
			relativePos.y = pair.Key.transform.position.y;
			spawnPoints.Add(pair.Key, relativePos);
		}
		m_spawnPoints = spawnPoints;
	}

	public static void RespawnPlayer(BaseCharacter player)
	{
		// if player is not in spawn point dictionary
		if (!m_spawnPoints.ContainsKey(player))
		{
			throw new System.Exception(string.Format("No spawn point assigned for player: {0}", player.name));
		}

		ms_instance.StartCoroutine(WaitToRespawn(player));
	}

	private static IEnumerator WaitToRespawn(BaseCharacter player)
	{
		yield return new WaitForSeconds(ms_instance.m_respawnDelay);
		player.SoftActivate();
		player.transform.position = m_spawnPoints[player];
	}
}
