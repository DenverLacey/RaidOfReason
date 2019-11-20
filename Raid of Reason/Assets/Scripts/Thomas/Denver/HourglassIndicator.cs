using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HourglassIndicator : MonoBehaviour
{
	[Tooltip("How long before hourglass starts to fade")]
	[SerializeField]
	private float m_fadeDelay = 0.3f;

	[Tooltip("How quickly hourglass fades")]
	[SerializeField]
	private float m_fadeDuration = 0.3f;

	private SkillManager m_skillManager;
	private SpriteRenderer m_hourglass;
	private BaseCharacter m_character;

	Vector3 m_offset;

	private float m_timer;
	private int m_phase;
	private bool m_alreadyActivated;
	private bool m_triggerDown;

	private Color m_startColour;
	private Color m_endColour;

	private Skills m_skill;

    // Start is called before the first frame update
    void Start()
    {
		m_skillManager = FindObjectOfType<SkillManager>();
		m_hourglass = GetComponent<SpriteRenderer>();
		m_character = transform.parent.GetComponentInChildren<BaseCharacter>();
		m_startColour = m_hourglass.color;
		m_endColour = m_hourglass.color;
		m_endColour.a = 0f;
		m_phase = 0;
		m_hourglass.enabled = false;

		Vector3 rotation = transform.eulerAngles;
		rotation.x = Camera.main.transform.eulerAngles.x;
		transform.eulerAngles = rotation;

		m_alreadyActivated = false;

		if (m_character == null)
		{
			gameObject.SetActive(false);
			return;
		}

		m_skill = m_skillManager.m_mainSkills[(int)m_character.CharacterType];

		m_skill.onDone += OnSkillCooldownDone;

		m_offset = transform.position - m_character.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		// move with character
		Vector3 position = transform.position;
		position.x = m_character.transform.position.x + m_offset.x;
		position.z = m_character.transform.position.z + m_offset.z;
		transform.position = position;

        if (XCI.GetAxis(XboxAxis.LeftTrigger, m_character.controller) > 0.01f && m_alreadyActivated == false && m_triggerDown == false)
		{
			m_alreadyActivated = true;
			m_triggerDown = true;
		}
		else if (XCI.GetAxis(XboxAxis.LeftTrigger, m_character.controller) > 0.01f && m_alreadyActivated && m_triggerDown == false)
		{
			m_phase = 1;
			m_hourglass.enabled = true;

			m_hourglass.DOKill();
			m_hourglass.color = m_startColour;

			m_timer = 0f;

			m_triggerDown = true;
		}
		else if (XCI.GetAxis(XboxAxis.LeftTrigger, m_character.controller) <= 0.01f)
		{
			m_triggerDown = false;
		}

		if (m_phase > 0)
		{
			m_timer += Time.deltaTime;
		}

		if (m_phase == 1 && m_timer >= m_fadeDelay)
		{
			m_hourglass.DOKill(true);
			m_hourglass.DOColor(m_endColour, m_fadeDuration);
			m_phase = 2;
		}

		if (m_phase == 2 && m_timer >= m_fadeDelay + m_fadeDuration)
		{
			m_phase = 0;
			m_hourglass.enabled = false;
		}
    }

	public void OnSkillCooldownDone()
	{
		m_alreadyActivated = false;
	}
}
