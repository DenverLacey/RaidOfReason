using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnEffectActor : MonoBehaviour
{
	public delegate void OnEvent(RespawnEffectActor respawnEffectActor);

	[Tooltip("Build up particle effect")]
	[SerializeField]
	private GameObject m_buildUpPrefab;

	[Tooltip("Respawn particle effect")]
	[SerializeField]
	private GameObject m_respawnPrefab;

	private float m_respawnEffectDelay;

	private float m_timer;

	public OnEvent onSpawn;
	public OnEvent onDeactivate;

	[HideInInspector]
	public BaseCharacter character;

	private void Start()
	{
		Deactivate();
	}

	// Update is called once per frame
	void Update()
    {
		m_timer += Time.deltaTime;

		if (m_timer >= m_respawnEffectDelay)
		{
			m_respawnPrefab.SetActive(true);
			onSpawn(this);
		}

		if (m_timer >= m_respawnEffectDelay + 4f)
		{
			Deactivate();
			onDeactivate(this);
		}
    }

	public void Activate(float respawnDelay)
	{
		gameObject.SetActive(true);
		m_buildUpPrefab.SetActive(true);
		m_respawnEffectDelay = respawnDelay; ;
	}

	public void Deactivate()
	{
		m_timer = 0f;
		character = null;
		m_buildUpPrefab.SetActive(false);
		m_respawnPrefab.SetActive(false);
		gameObject.SetActive(false);
	}
}
