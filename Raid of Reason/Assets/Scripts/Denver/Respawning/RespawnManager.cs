/*
 * Author: Denver
 * Description:	Handles all respawn mechanics. Keeps track of who's respawning and where they should respawn.
 *				Also will activate death screen when all players are dead (respawning)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all respawn mechanics
/// </summary>
public class RespawnManager : MonoBehaviour
{
	/// <summary>
	/// Contains a players respawn information
	/// </summary>
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

	[Tooltip("Respawn Effect Prefab")]
	[SerializeField]
	private GameObject m_respawnEffectObject;

	private static RespawnManager ms_instance;
	private static Dictionary<BaseCharacter, RespawnInformation> m_respawnInformation;

	private static Queue<RespawnEffectActor> m_inactiveRespawnEffects;
	private static List<RespawnEffectActor> m_activeRespawnEffects;

	private void Awake()
	{
		ms_instance = this;
	}

	private void Start()
	{
		m_respawnInformation = new Dictionary<BaseCharacter, RespawnInformation>();
		m_inactiveRespawnEffects = new Queue<RespawnEffectActor>();
		m_activeRespawnEffects = new List<RespawnEffectActor>();

		foreach (var player in GameManager.Instance.AllPlayers)
		{
			m_respawnInformation.Add(player, new RespawnInformation(player.transform.position));
			RespawnEffectActor respawnEffectActor = Instantiate(m_respawnEffectObject).GetComponent<RespawnEffectActor>();

			respawnEffectActor.onSpawn = OnSpawnEvent;
			respawnEffectActor.onDeactivate = OnDeactivate;

			m_inactiveRespawnEffects.Enqueue(respawnEffectActor);
		}
	}

    private static IEnumerator DisplayDeathScreen()
    {
        yield return new WaitForSecondsRealtime(ms_instance.m_deathScreenDelay);
        ms_instance.m_stats.SetActive(false);
        ms_instance.m_deathScreen.DeathScreen();
    }

	/// <summary>
	/// Updates respawn points for all players
	/// </summary>
	/// <param name="spawnPoint"> New respawn point </param>
	public static void UpdateSpawnPoint(Vector3 spawnPoint)
	{
		var respawnInfomation = new Dictionary<BaseCharacter, RespawnInformation>();
		foreach (var player in GameManager.Instance.AllPlayers)
		{
			Vector3 relativePosition = GetRelativePosition(player, spawnPoint);
			respawnInfomation.Add(player, new RespawnInformation(relativePosition));
		}
		m_respawnInformation = respawnInfomation;
	}

	/// <summary>
	/// Respawns a player after a time
	/// </summary>
	/// <param name="player"> player to respawn </param>
	public static void RespawnPlayer(BaseCharacter player)
	{
        if (m_respawnInformation[player].isRespawning == false)
        {
		    ms_instance.StartCoroutine(WaitToRespawn(player));
		    m_respawnInformation[player].isRespawning = true;
        }

        if (AllRespawning())
        {
            ms_instance.StartCoroutine(DisplayDeathScreen());
        }
	}

	/// <summary>
	/// Activates respawn effect particle system after respawn delay
	/// </summary>
	/// <param name="player"> the player that is being respawned </param>
	/// <returns> WaitForSeconds </returns>
	private static IEnumerator WaitToRespawn(BaseCharacter player)
	{
		yield return new WaitForSeconds(ms_instance.m_respawnDelay);

		RespawnEffectActor respawnEffect = m_inactiveRespawnEffects.Dequeue();
		respawnEffect.Activate(2f);
		respawnEffect.character = player;
		respawnEffect.transform.position = m_respawnInformation[player].respawnPosition;

		m_activeRespawnEffects.Add(respawnEffect);
	}

	/// <summary>
	/// Checks if all players are respawning
	/// </summary>
	/// <returns> true if all players are respawing. False if otherwise </returns>
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

	/// <summary>
	/// Returns a Vector3 position that maintains the character's Y position
	/// </summary>
	/// <param name="character"> the pertinent character </param>
	/// <param name="source"> the position that gives the relative position x and z coords </param>
	/// <returns> A Vector3 with source's x and z coords but the character's y coord </returns>
	private static Vector3 GetRelativePosition(BaseCharacter character, Vector3 source)
	{
        source.y = character.YPosition;
		return source;
	}

	/// <summary>
	/// Respawns a player and updates respawn information. Is called by a respawn effect actor when it hits a spawn event
	/// </summary>
	/// <param name="respawnEffectActor"> respawn effect that has hit a spawn event </param>
	private void OnSpawnEvent(RespawnEffectActor respawnEffectActor)
	{
		BaseCharacter character = respawnEffectActor.character;

		character.SoftActivate();

        character.transform.position = m_respawnInformation[character].respawnPosition;

        m_respawnInformation[character].isRespawning = false;
	}

	/// <summary>
	/// Moves a respawnEffectActor from the active list to the inactive queue
	/// </summary>
	/// <param name="respawnEffectActor"> respawn effect to move </param>
	private void OnDeactivate(RespawnEffectActor respawnEffectActor)
	{
		m_activeRespawnEffects.Remove(respawnEffectActor);
		m_inactiveRespawnEffects.Enqueue(respawnEffectActor);
	}
}
