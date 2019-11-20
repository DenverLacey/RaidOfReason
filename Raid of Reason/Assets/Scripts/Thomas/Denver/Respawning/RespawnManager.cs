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

	public static void RespawnPlayer(BaseCharacter player)
	{
		// if player is not in spawn point dictionary
		if (!m_respawnInformation.ContainsKey(player))
		{
			Vector3 spawnPoint = GetRelativePosition(player, m_respawnInformation.Values.GetEnumerator().Current.respawnPosition);
			m_respawnInformation.Add(player, new RespawnInformation(spawnPoint));
		}

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

	private static IEnumerator WaitToRespawn(BaseCharacter player)
	{
		yield return new WaitForSeconds(ms_instance.m_respawnDelay);

		RespawnEffectActor respawnEffect = m_inactiveRespawnEffects.Dequeue();
		respawnEffect.Activate(2f);
		respawnEffect.character = player;
		respawnEffect.transform.position = m_respawnInformation[player].respawnPosition;

		m_activeRespawnEffects.Add(respawnEffect);

        AkSoundEngine.PostEvent("Respawn_Event", respawnEffect.gameObject);
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
        source.y = character.YPosition;
		return position;
	}

	private void OnSpawnEvent(RespawnEffectActor respawnEffectActor)
	{

        BaseCharacter character = respawnEffectActor.character;

		character.SoftActivate();

        Vector3 desiredPosition = m_respawnInformation[character].respawnPosition;
        desiredPosition.y = character.YPosition;
        character.transform.position = desiredPosition;

        m_respawnInformation[character].isRespawning = false;
	}

	private void OnDeactivate(RespawnEffectActor respawnEffectActor)
	{
		m_activeRespawnEffects.Remove(respawnEffectActor);
		m_inactiveRespawnEffects.Enqueue(respawnEffectActor);
	}
}
