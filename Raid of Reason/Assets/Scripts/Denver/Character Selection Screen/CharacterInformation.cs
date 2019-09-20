using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterInformation : InteractableUIElement
{
	[SerializeField]
	[Tooltip("Character that information is about")]
	private CharacterType m_character;
	public CharacterType Character { get => m_character; }

	[Header("Punch Tweening")]
	[SerializeField]
	[Tooltip("How far panel will bob when clicked")]
	private float m_punchForce = 3f;

	[SerializeField]
	[Tooltip("How long panel will bob for in seconds")]
	private float m_punchDuration = 0.3f;

	[SerializeField]
	[Tooltip("How much the panel's bob will vibrate")]
	private int m_punchVibrato = 10;

	[SerializeField]
	[Tooltip("Elasticity of the bob")]
	[Range(0.0f, 1.0f)]
	private float m_punchElasticity = 1.0f;

	[Header("Border Colour Tweening")]
	[SerializeField]
	[Tooltip("How long it takes for colour to change to target colour")]
	private float m_colourDuration = 0.7f;

	[Header("SFX")]
	[SerializeField]
	[Tooltip("Sound played when selected")]
	private SoundData m_selectedSound;

	[SerializeField]
	[Tooltip("Sound played when selection attempted but failed")]
	private SoundData m_failedSound;

	private bool m_selected;

	// child objects
	private Image m_selectedBorder;
	private Color m_selectedBorderColour;

	private Image[] m_childImages;
	private List<Color> m_childImageOriginalColours = new List<Color>();
	int m_hoverers;

	private void Start()
	{
		m_selectedBorder = transform.Find("SelectedBorder").GetComponent<Image>();
		m_selectedBorderColour = m_selectedBorder.color;

		m_childImages = GetComponentsInChildren<Image>();

		foreach (Image child in m_childImages)
		{
			m_childImageOriginalColours.Add(child.color);
			child.color *= Color.gray;
		}
	}

	public void Hover()
	{
		Interlocked.Increment(ref m_hoverers);

		for (int i = 0; i < m_childImages.Length; i++)
		{
			m_childImages[i].color = m_childImageOriginalColours[i];
		}
	}

	public void Unhover()
	{
		Interlocked.Decrement(ref m_hoverers);

		if (m_hoverers <= 0 && !m_selected)
		{
			m_hoverers = 0;
			foreach (Image child in m_childImages)
			{
				child.color *= Color.gray;
			}
		}
	}

	public bool SelectCharacter(ref CharacterType character, Color tweenColour)
	{
		if (!m_selected)
		{
			// set character 
			character = m_character;

			// kill tweening
			transform.DOKill(complete: true);
			transform.DOPunchPosition(Vector3.down * m_punchForce, m_punchDuration, m_punchVibrato, m_punchElasticity);

			// TODO: Make nice sound
			AudioManager.Instance.PlaySound(m_selectedSound);

			m_selectedBorder.DOColor(tweenColour, m_colourDuration).SetLoops(-1, LoopType.Yoyo);

			m_selected = true;

			return true;
		}
		else
		{
			// TODO: Make bad sound
			AudioManager.Instance.PlaySound(m_failedSound);

			return false;
		}
	}

	public bool DeselectCharacter()
	{
		if (m_selected)
		{
			m_selected = false;

			// do transform tweeing
			transform.DOKill(complete: true);

			transform.DOPunchPosition(Vector3.up * m_punchForce, m_punchDuration, m_punchVibrato, m_punchElasticity);

			// stop tweening selectedBorder colour
			m_selectedBorder.DOKill();
			m_selectedBorder.color = m_selectedBorderColour;

			if (m_hoverers == 0)
			{
				foreach (Image child in m_childImages)
				{
					child.color *= Color.gray;
				}
			}

			return true;
		}
		else
		{
			return false;
		}
	}
}
