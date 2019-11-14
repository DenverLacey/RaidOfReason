/*
 * Author: Denver
 * Description:	Handles respawn particle system timings and notifies RespawnManager when to spawn the player and when the particle is done
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles functionality of the Respawn Particle Effect
/// </summary>
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

    private int m_phase;

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
        if (m_phase > 0)
        {
            m_timer += Time.deltaTime;
        }

		if (m_phase == 1 && m_timer >= m_respawnEffectDelay)
		{
			m_respawnPrefab.SetActive(true);
			onSpawn(this);
            m_phase = 2;
		}

		if (m_phase == 2 && m_timer >= m_respawnEffectDelay + 4f)
		{
			Deactivate();
			onDeactivate(this);
		}
    }

	public void Activate(float respawnDelay)
	{
        m_phase = 1;

		gameObject.SetActive(true);
		m_buildUpPrefab.SetActive(true);
		m_respawnEffectDelay = respawnDelay; ;
	}

	public void Deactivate()
	{
		m_timer = 0f;
        m_phase = 0;
		character = null;
		m_buildUpPrefab.SetActive(false);
		m_respawnPrefab.SetActive(false);
		gameObject.SetActive(false);
	}
}
