using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class TauntEffectIndicator : MonoBehaviour
{
	[Tooltip("Duration of the effect")]
	[SerializeField]
	private float m_duration = 1.6f;

	[Tooltip("Fade in time, in seconds")]
	[SerializeField]
	private float m_fadeInTime = 0.2f;

	[Tooltip("Fade out time, in seconds")]
	[SerializeField]
	private float m_fadeOutTime = 0.4f;

	private float m_timer;
	private int m_phase;
	private Vector3 m_AOEScale;
	private SpriteRenderer m_renderer;
	private Vector3 m_scaleMultiplier;

    // Start is called before the first frame update
    void Start()
    {
		m_duration -= m_fadeInTime + m_fadeOutTime;
		m_renderer = GetComponent<SpriteRenderer>();

		Color noAlpha = m_renderer.color;
		noAlpha.a = 0;
		m_renderer.color = noAlpha;

		gameObject.SetActive(false);
		m_scaleMultiplier = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
		// fade in
		if (m_phase == 1)
		{
			m_renderer.DOFade(1, m_fadeInTime);
			m_phase = 2;
		}

		// wait for fade out
		if (m_phase == 2 && m_renderer.color.a == 1)
		{
			m_timer += Time.deltaTime;

			if (m_timer >= m_duration)
			{
				m_phase = 3;
			}
		}

		// fade out
		if (m_phase == 3)
		{
			m_renderer.DOFade(0, m_fadeOutTime);
			m_phase = 4;
		}

		// reset
		if (m_phase == 4 && m_renderer.color.a == 0)
		{
			m_timer = 0f;
			m_phase = 0;
			gameObject.SetActive(false);
		}
    }

	public void Show(float radius, Vector3 position)
	{
		gameObject.SetActive(true);
		m_phase = 1;

		Vector3 desiredScale = new Vector3(radius, radius, 0);
		desiredScale.x *= m_scaleMultiplier.x;
		desiredScale.y *= m_scaleMultiplier.y;
		transform.localScale = desiredScale;

		position.y = 0.1f;
		transform.position = position;
	}
}
