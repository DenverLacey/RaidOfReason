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
	private Character m_character;
	public Character Character { get => m_character; }

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

	private bool m_selected;

	private Vector3 m_originalPosition;

	// child objects
	private Image m_artwork;
	private Image m_border;
	private Image m_selectedBorder;
	private Transform m_selectedTick;

	private Image[] m_childImages;
	private List<Color> m_childImageOriginalColours = new List<Color>();
	int m_hoverers;

	private void Start()
	{
		m_artwork = transform.Find("CharacterCell").Find("Artwork Mask").Find("artwork").GetComponent<Image>();
		m_border = transform.Find("CharacterCell").Find("border").GetComponent<Image>();
		m_selectedBorder = transform.Find("CharacterCell").Find("selectedBorder").GetComponent<Image>();
		m_selectedTick = transform.Find("CharacterCell").Find("selectedTick");

		m_childImages = GetComponentsInChildren<Image>();

		foreach (Image child in m_childImages)
		{
			m_childImageOriginalColours.Add(child.color);
			child.color *= Color.gray;
		}

		m_originalPosition = transform.position;
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

	public bool SelectCharacter(ref Character character, Color tweenColour)
	{
		if (!m_selected)
		{
			// set character 
			character = m_character;

			// kill tweening
			transform.DOKill();
			transform.position = m_originalPosition;
			transform.DOPunchPosition(Vector3.down * m_punchForce, m_punchDuration, m_punchVibrato, m_punchElasticity);

			// TODO: Make nice sound

			// tween selectedBorder colour
			m_selectedBorder.DOColor(tweenColour, m_colourDuration).SetLoops(-1);

			// tween selectedTick position in
			m_selectedTick.localPosition = new Vector3(400, 0, 0);
			m_selectedTick.DOLocalMoveX(0, .1f);

			m_selected = true;

			return true;
		}
		else
		{
			// TODO: Make bad sound
			return false;
		}
	}

	public bool DeselectCharacter()
	{
		if (m_selected)
		{
			m_selected = false;

			// do transform tweeing
			transform.DOKill();
			transform.position = m_originalPosition;

			transform.DOPunchPosition(Vector3.up * m_punchForce, m_punchDuration, m_punchVibrato, m_punchElasticity);

			// stop tweening selectedBorder colour
			m_selectedBorder.DOKill();
			m_selectedBorder.color = Color.clear;

			// tween selectedTick position out
			m_selectedTick.DOLocalMoveX(-400, .1f);

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
