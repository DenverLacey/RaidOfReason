/*
 * Author: Denver
 * Description:	Handles all UI functionality for CharacterInformation UI Element
 */

using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Handles all UI functionality for CharacterInformation UI Element
/// </summary>
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

	[Header("Border Tweening")]
	[SerializeField]
	[Tooltip("How long it takes for colour to change to target colour")]
	private float m_colourDuration = 0.7f;

	[SerializeField]
	[Tooltip("How long it takes for the information panel / selected overlay to pop up")]
	private float m_selectedOverlayDuration = 0.3f;

	[SerializeField]
	[Tooltip("Where to hide the information panel / selected overlay")]
	private Vector3 m_selectedOverlayHidePosition;

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

	private Transform m_characterImage;
	private Transform m_characterImageGrey;

	private Transform m_selectedOverlay;
	private Vector3 m_selectedOverlayShowPosition;

	int m_hoverers;

	private void Start()
	{
		m_selectedBorder = transform.parent.Find("SelectedBorder").Find("Image").GetComponent<Image>();
		m_selectedBorderColour = m_selectedBorder.color;

		m_characterImage = transform.parent.Find("Character Image");
		m_characterImageGrey = transform.parent.Find("Character Image Grey");

		m_selectedOverlay = transform.parent.Find("Information Panel");
		m_selectedOverlayShowPosition = m_selectedOverlay.position;
		m_selectedOverlay.position = m_selectedOverlayHidePosition;
	}

	/// <summary>
	/// Incremenets number of hoverers. Resets all child image's colours
	/// </summary>
	public void Hover()
	{
		//Interlocked.Increment(ref m_hoverers);
		m_hoverers++;

		m_characterImageGrey.gameObject.SetActive(false);
		m_characterImage.gameObject.SetActive(true);
	}

	/// <summary>
	/// Decrements number of hoverers. Grays out child images if no hoverers
	/// </summary>
	public void Unhover()
	{
		//Interlocked.Decrement(ref m_hoverers);
		m_hoverers--;

		if (m_hoverers <= 0 && !m_selected)
		{
			m_hoverers = 0;
			m_characterImage.gameObject.SetActive(false);
			m_characterImageGrey.gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// Selects a character for a player and locks UI element
	/// </summary>
	/// <param name="character">
	/// Character variable that is changed
	/// </param>
	/// <param name="tweenColour">
	/// What colour the selected border should be
	/// </param>
	/// <returns>
	/// If selection was successful
	/// </returns>
	public bool SelectCharacter(ref CharacterType character, Color tweenColour)
	{
		if (!m_selected)
		{
            // set character 
            character = m_character;

			// kill tweening
			m_characterImage.DOKill(complete: true);
			m_characterImage.DOPunchPosition(Vector3.down * m_punchForce, m_punchDuration, m_punchVibrato, m_punchElasticity);

			m_selectedOverlay.DOKill();
			m_selectedOverlay.DOMove(m_selectedOverlayShowPosition, m_selectedOverlayDuration);

			// make nice sound
			AudioManager.Instance.PlaySound(m_selectedSound);

			m_selectedBorder.DOColor(tweenColour, m_colourDuration).SetLoops(-1, LoopType.Yoyo);

			m_selected = true;

			return true;
		}
		else
		{
			// make bad sound
			AudioManager.Instance.PlaySound(m_failedSound);

			return false;
		}
	}

	/// <summary>
	/// Turns off selected border and unlocks UI Element
	/// </summary>
	/// <returns>
	/// Returns true if deselect was successful
	/// </returns>
	public bool DeselectCharacter()
	{
		if (m_selected)
		{
			m_selected = false;

			// do transform tweeing
			m_characterImage.DOKill(complete: true);

			m_characterImage.DOPunchPosition(Vector3.up * m_punchForce, m_punchDuration, m_punchVibrato, m_punchElasticity);

			// stop tweening selectedBorder colour
			m_selectedBorder.DOKill();
			m_selectedBorder.color = m_selectedBorderColour;

			m_selectedOverlay.DOKill();
			m_selectedOverlay.DOMove(m_selectedOverlayHidePosition, m_selectedOverlayDuration * 5f);

			if (m_hoverers == 0)
			{
				m_characterImage.gameObject.SetActive(false);
				m_characterImageGrey.gameObject.SetActive(true);
			}

			return true;
		}
		else
		{
			return false;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var cursor = collision.GetComponent<PlayerCursor>();

		if (cursor)
		{
			Hover();
			cursor.SetCollidedTransform(transform);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var cursor = collision.GetComponent<PlayerCursor>();

		if (cursor)
		{
			Unhover();
			cursor.NullCollidedTransform();
		}
	}
}
