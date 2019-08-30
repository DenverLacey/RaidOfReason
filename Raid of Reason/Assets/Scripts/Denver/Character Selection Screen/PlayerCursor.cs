using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using XboxCtrlrInput;

public class PlayerCursor : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How fast the cursor will move")]
	private float m_speed;

	[SerializeField]
	[Tooltip("How quickly the token will move")]
	private float m_tokenSpeed = 0.5f;

	[Tooltip("Token Object")]
	public Transform token;

	[SerializeField]
	[Tooltip("Colour selected border will go when this clicks it")]
	Color m_tweenColour;

	[HideInInspector]
	public bool hasToken;

	[HideInInspector]
	public XboxController controller = XboxController.Any;

	private Character m_selectedCharacter;
	private bool m_characterSelected;

	private Transform m_collidedTransform = null;
	private Transform m_currentCharInfoTransform = null;

	// Start is called before the first frame update
	void Start()
    {
		hasToken = true;
	}

    // Update is called once per frame
    void Update()
    {
		// if no controller assigned
		if (controller == XboxController.Any)
		{
			return;
		}

		DoMovement();
		DoTokenAndCharacterSelect();
    }

	/// <summary>
	/// moves cursor
	/// </summary>
	void DoMovement()
	{
		// get input
		float x = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float y = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		// scale input by speed and time
		x *= m_speed * Time.deltaTime;
		y *= m_speed * Time.deltaTime;

		transform.Translate(x, y, 0);
	}

	/// <summary>
	/// moves token with cursor
	/// </summary>
	void DoTokenAndCharacterSelect()
	{
		// select character
		if (XCI.GetButtonDown(XboxButton.A, controller) &&
			m_collidedTransform != null)
		{
			InteractableUIElement temp = m_collidedTransform.GetComponent<InteractableUIElement>();

			if (temp is CharacterInformation && hasToken)
			{
				if (((CharacterInformation)temp).SelectCharacter(ref m_selectedCharacter, m_tweenColour))
				{
					m_currentCharInfoTransform = m_collidedTransform;
					m_characterSelected = true;
					hasToken = false;
				}
			}
			else if (temp is CharacterSelection && !hasToken)
			{
				((CharacterSelection)temp).OnPressed();
			}
		}

		// deselect character
		if (XCI.GetButtonDown(XboxButton.B, controller) &&
			m_currentCharInfoTransform != null)
		{
			if (m_currentCharInfoTransform.GetComponent<CharacterInformation>().DeselectCharacter())
			{
				m_characterSelected = false;
				hasToken = true;
				m_currentCharInfoTransform = null;
			}
		}

		if (hasToken)
		{
			token.position = Vector3.Lerp(token.position, transform.position, m_tokenSpeed);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		m_collidedTransform = collision.transform;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		m_collidedTransform = null;
	}

	public (XboxController controller, Character selectedCharacter, bool characterSelected) GetSelectedCharacter()
	{
		return (controller, selectedCharacter: m_selectedCharacter, characterSelected: m_characterSelected);
	}

	public void Activate()
	{
		gameObject.SetActive(true);
		token.gameObject.SetActive(true);
	}

	public void Deactivate(Vector3 inactivePosition)
	{
		// move to inactive position
		transform.position = inactivePosition;
		token.position = inactivePosition;

		// deselect character
		if (m_characterSelected)
		{
			m_currentCharInfoTransform.GetComponent<CharacterInformation>().DeselectCharacter();
			m_characterSelected = false;
			hasToken = true;
			m_currentCharInfoTransform = null;
		}

		gameObject.SetActive(false);
		token.gameObject.SetActive(false);
	}

	public bool HasSelectedCharacter()
	{
		return m_characterSelected;
	}
}
