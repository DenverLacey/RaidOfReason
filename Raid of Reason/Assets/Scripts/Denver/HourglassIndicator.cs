using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using DG.Tweening;

public class HourglassIndicator : MonoBehaviour
{
	[Tooltip("How long before hourglass starts to fade")]
	[SerializeField]
	private float m_fadeDelay = 0.3f;

	[Tooltip("How quickly hourglass fades")]
	[SerializeField]
	[Range(0f, 1f)]
	private float m_fadeDuration;

	private SkillManager m_skillManager;
	private SpriteRenderer m_hourglass;
	private BaseCharacter m_character;

	private float m_timer;
	private bool m_show;

	private Color m_endColour;

    // Start is called before the first frame update
    void Start()
    {
		m_skillManager = FindObjectOfType<SkillManager>();
		m_hourglass = GetComponent<SpriteRenderer>();
		m_character = GetComponentInParent<BaseCharacter>();
		m_endColour = m_hourglass.color;
		m_endColour.a = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (XCI.GetAxis(XboxAxis.LeftTrigger, m_character.controller) > 0.01f && m_skillManager.m_mainSkills[(int)m_character.CharacterType].onCooldown)
		{
			m_show = true;
		}

		if (m_show)
		{
			m_timer += Time.deltaTime;

			if (m_timer >= m_fadeDelay)
			{
				m_hourglass.DOColor(m_endColour, m_fadeDuration);
			}
		}

		// if (m_timer)
    }
}
