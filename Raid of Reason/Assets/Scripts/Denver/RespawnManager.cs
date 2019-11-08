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
	private static List<RespawnInformation> m_respawnInformation = new List<RespawnInformation>();

	private void Awake()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
		}
	}

	private void Start()
	{
		foreach (var player in GameManager.Instance.Players)
		{
			m_respawnInformation.Add(new RespawnInformation(player, player.transform.position));
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

        if (m_respawnInformation.TrueForAll(i => i.isRespawning))
        {
            ms_instance.StartCoroutine(DisplayDeathScreen());
        }
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
