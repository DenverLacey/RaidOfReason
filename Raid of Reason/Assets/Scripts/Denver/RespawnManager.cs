using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
	private class RespawnInformation
	{
		public Vector3 respawnPosition;
		public bool isRespawning;

		public RespawnInformation(Vector3 a_respawnPosition)
		{
			respawnPosition = a_respawnPosition;
			isRespawning = false;
		}
	}

	[Tooltip("Respawn Delay")]
	[SerializeField]
	private float m_respawnDelay = 5f;

    [Tooltip("Delay in between all players dying and the death screen displaying")]
    [SerializeField]
    private float m_deathScreenDelay = 1f;

    [Tooltip("Stats UI")]
    [SerializeField]
    private GameObject m_stats;

    [Tooltip("Death Screen (called EndMenu)")]
    [SerializeField]
    private DeathMenu m_deathScreen;

	private static RespawnManager ms_instance;
	private static Dictionary<BaseCharacter, RespawnInformation> m_respawnInformation = new Dictionary<BaseCharacter, RespawnInformation>();

	private void Awake()
	{
		ms_instance = this;
	}

	private void Start()
	{
		foreach (var player in GameManager.Instance.Players)
		{
			m_respawnInformation.Add(player, new RespawnInformation(player.transform.position));
		}
	}

    private static IEnumerator DisplayDeathScreen()
    {
        yield return new WaitForSecondsRealtime(ms_instance.m_deathScreenDelay);
        ms_instance.m_stats.SetActive(false);
        ms_instance.m_deathScreen.DeathScreen();
    }

	public static void UpdateSpawnPoint(Vector3 spawnPoint)
	{
		var respawnInfomation = new Dictionary<BaseCharacter, RespawnInformation>();
		foreach (var player in GameManager.Instance.Players)
		{
			Vector3 relativePosition = GetRelativePosition(player, spawnPoint);
			respawnInfomation.Add(player, new RespawnInformation(relativePosition));
		}
		m_respawnInformation = respawnInfomation;
	}

	public static void RespawnPlayer(BaseCharacter player)
	{
		// if player is not in spawn point dictionary
		if (!m_respawnInformation.ContainsKey(player))
		{
			Vector3 spawnPoint = GetRelativePosition(player, m_respawnInformation.Values.GetEnumerator().Current.respawnPosition);
			m_respawnInformation.Add(player, new RespawnInformation(spawnPoint));
		}

		ms_instance.StartCoroutine(WaitToRespawn(player));
		m_respawnInformation[player].isRespawning = true;

        if (AllRespawning())
        {
            ms_instance.StartCoroutine(DisplayDeathScreen());
        }
	}

	private static IEnumerator WaitToRespawn(BaseCharacter player)
	{
		yield return new WaitForSeconds(ms_instance.m_respawnDelay);
		player.SoftActivate();
		player.transform.position = m_respawnInformation[player].respawnPosition;
	}

	private static bool AllRespawning()
	{
		foreach (var pair in m_respawnInformation)
		{
			if (pair.Value.isRespawning == false)
			{
				return false;
			}
		}
		return true;
	}

	private static Vector3 GetRelativePosition(BaseCharacter character, Vector3 source)
	{
		Vector3 position = source;
		source.y = character.transform.position.y;
		return position;
	}
}
