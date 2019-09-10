using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMesh))]
public class DamageIndicator : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How long text will be displayed")]
	private float m_duration;

	[SerializeField]
	[Tooltip("Speed at which text will move")]
	private float m_moveSpeed;

	[SerializeField]
	[Tooltip("Direction text will move in")]
	private Vector3 m_moveDirecion;

	[SerializeField]
	[Tooltip("Curve that controls texts alpha over time")]
	private AnimationCurve m_alphaCurve;

	[SerializeField]
	[Tooltip("Curve that controls speed of text over time")]
	private AnimationCurve m_speedCurve;

	[SerializeField]
	[Tooltip("Gradient of colours")]
	private Gradient m_damageGradient;

	[SerializeField]
	[Tooltip("Maximum amount of damage that can be dealt to an enemy")]
	private float m_maxDamage;
	
	private TextMesh m_textMesh;

	private float m_lifetimeTimer;

	public void Init(float damageDealt)
	{
		m_textMesh = GetComponent<TextMesh>();
		m_lifetimeTimer = 0.0f;

		// set text
		m_textMesh.text = damageDealt.ToString();

		// set colour
		float percentage = damageDealt / m_maxDamage;
		m_textMesh.color = m_damageGradient.Evaluate(percentage);
	}

	// Update is called once per frame
	void Update()
    {
		float lifetimePercentage = m_lifetimeTimer / m_duration;
		float alpha = m_alphaCurve.Evaluate(lifetimePercentage);

		// orient text
		transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
		transform.Translate(m_moveDirecion * m_speedCurve.Evaluate(lifetimePercentage) * m_moveSpeed * Time.deltaTime);

		// run lifetime
		m_lifetimeTimer += Time.deltaTime;

		if (m_lifetimeTimer >= m_duration)
		{
			Destroy(gameObject);
		}

		// change colour
		m_textMesh.color = new Color(
			m_textMesh.color.r,
			m_textMesh.color.g,
			m_textMesh.color.b,
			alpha
		);
	}
}
