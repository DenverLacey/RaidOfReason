using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	private class RespawnInformation
	{
		public BaseCharacter character;
		public Vector3 respawnPosition;
		public bool isRespawning;

		public RespawnInformation(BaseCharacter a_character, Vector3 a_respawnPosition)
		{
			character = a_character;
			respawnPosition = a_respawnPosition;
			isRespawning = false;
		}
	}

	[Tooltip("Respawn Delay")]
	[SerializeField]
	private float m_respawnDelay = 5f;

	private static RespawnManager ms_instance;

	private static List<RespawnInformation> m_respawnInformation = new List<RespawnInformation>();

	private void Awake()
	{
		if (ms_instance == null)
		{
			ms_instance = this;

			foreach (var player in GameManager.Instance.Players)
			{
				m_respawnInformation.Add(new RespawnInformation(player, player.transform.position));
			}

			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if (m_respawnInformation.TrueForAll(i => i.isRespawning))
		{
			// death screen
		}
	}

	public static void UpdateSpawnPoint(Vector3 spawnPoint)
	{
		for (int i = 0; i < m_respawnInformation.Count; i++)
		{
			var info = m_respawnInformation[i];

			Vector3 relativePosition = spawnPoint;
			relativePosition.y = info.character.transform.position.y;
			info.respawnPosition = relativePosition;
		}
		
	}

	public static void RespawnPlayer(BaseCharacter player)
	{
		// if player is not in spawn point dictionary
		if (!m_respawnInformation.Exists(i => i.character == player))
		{
			throw new System.Exception(string.Format("No spawn point assigned for player: {0}", player.name));
		}

		ms_instance.StartCoroutine(WaitToRespawn(player));
		Get(player).isRespawning = true;
	}

	private static IEnumerator WaitToRespawn(BaseCharacter player)
	{
		yield return new WaitForSeconds(ms_instance.m_respawnDelay);
		player.SoftActivate();
		player.transform.position = Get(player).respawnPosition;
	}

	private static RespawnInformation Get(BaseCharacter character)
	{
		return m_respawnInformation.Find(i => i.character == character);
	}
}
