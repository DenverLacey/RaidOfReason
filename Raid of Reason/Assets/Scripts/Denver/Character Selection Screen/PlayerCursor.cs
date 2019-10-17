/*
 * Author: Denver
 * Description:	Handles all functionality for player cursor's in the Character Selection Screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using XboxCtrlrInput;
using XInputDotNetPure;

/// <summary>
/// Handles all functionality for player cursor's in the Character Selection Screen
/// </summary>
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

	[SerializeField]
	[Tooltip("Rumble intensity")]
	private float m_rumbleIntensity = 1000f;

	[SerializeField]
	[Tooltip("Duration of rumble")]
	private float m_rumbleDuration = 0.1f;

	[HideInInspector]
	public bool hasToken;

	[HideInInspector]
	public XboxController controller = XboxController.Any;
	private PlayerIndex m_playerIndex;

	private CharacterType m_selectedCharacter;
	private bool m_characterSelected;

	private Transform m_collidedTransform = null;
	private Transform m_currentCharInfoTransform = null;

	private Vector3 m_inactivePosition;

	private CanvasScaler m_canvas;

	// Start is called before the first frame update
	void Start()
    {
		hasToken = true;

		m_canvas = FindObjectOfType<CanvasScaler>();

		m_inactivePosition = transform.position;

		switch (controller)
		{
			case XboxController.First:
				m_playerIndex = PlayerIndex.One;
				break;

			case XboxController.Second:
				m_playerIndex = PlayerIndex.Two;
				break;

			case XboxController.Third:
				m_playerIndex = PlayerIndex.Three;
				break;
		}
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
		x *= m_speed * Time.unscaledDeltaTime;
		y *= m_speed * Time.unscaledDeltaTime;

		transform.Translate(x, y, 0);

		// clamp to screen
		float widthExtents = m_canvas.referenceResolution.x * m_canvas.scaleFactor / 2f;
		float heightExtents = m_canvas.referenceResolution.y * m_canvas.scaleFactor / 2f;

		Vector3 desiredPosition = transform.localPosition;
		desiredPosition.x = Mathf.Clamp(desiredPosition.x, -widthExtents, widthExtents);
		desiredPosition.y = Mathf.Clamp(desiredPosition.y, -heightExtents, heightExtents);

		transform.localPosition = desiredPosition; 
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
					DoRumble();
					m_currentCharInfoTransform = m_collidedTransform;
					m_characterSelected = true;
					hasToken = false;
				}
			}
			else if (temp is CharacterSelection && !hasToken)
			{
				((CharacterSelection)temp).OnPressed();
			}
            else if (XCI.GetButtonDown(XboxButton.A, XboxController.First) && temp is BackButton)
            {
                // load main menu scene
                ((BackButton)temp).OnPressed();
            }
		}

		// deselect character
		if (XCI.GetButtonDown(XboxButton.B, controller) &&
			m_currentCharInfoTransform != null)
		{
			if (m_currentCharInfoTransform.GetComponent<CharacterInformation>().DeselectCharacter())
			{
				DoRumble();

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

	/// <summary>
	/// Sets current collided transform and calls Character Information's Hover function
	/// if collision is a Character Information Object
	/// </summary>
	/// <param name="collision">
	/// Collider of the object that has been collided with
	/// </param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		m_collidedTransform = collision.transform;

		var info = m_collidedTransform.GetComponent<CharacterInformation>();

		if (info)
		{
			info.Hover();
		}
	}

	/// <summary>
	/// nulls current collided transform and calls Character Information's Unhover function
	/// if collision is a Character Information Object
	/// </summary>
	/// <param name="collision">
	/// Collider of the object that has been collided with
	/// </param>
	private void OnTriggerExit2D(Collider2D collision)
	{
		var info = m_collidedTransform?.GetComponent<CharacterInformation>();

		if (info)
		{
			info.Unhover();
		}

		m_collidedTransform = null;
	}

	/// <summary>
	/// Gets all relavent information for character selection for a player
	/// </summary>
	/// <returns>
	/// All relavent information for character selection for a player
	/// </returns>
	public (XboxController controller, PlayerIndex playerIndex, CharacterType selectedCharacter, bool characterSelected) GetSelectedCharacter()
	{
		return (controller, m_playerIndex, m_selectedCharacter, m_characterSelected);
	}

	/// <summary>
	/// Activates a Cursor and its token
	/// </summary>
	public void Activate()
	{
		gameObject.SetActive(true);
		token.gameObject.SetActive(true);
	}

	/// <summary>
	/// Deactivates a Cursor and its token. Also moves cursor and token to their
	/// inactive positions
	/// </summary>
	public void Deactivate()
	{
		// move to inactive position
		transform.position = m_inactivePosition;
		token.position = m_inactivePosition;

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

	/// <summary>
	/// If player has selected a character
	/// </summary>
	/// <returns>
	/// True if player has selected a character. False if otherwise
	/// </returns>
	public bool HasSelectedCharacter()
	{
		return m_characterSelected;
	}

	/// <summary>
	/// Rubmles player's controller
	/// </summary>
	public void DoRumble()
	{
		GamePad.SetVibration(m_playerIndex, m_rumbleIntensity, m_rumbleIntensity);
		StartCoroutine(StopRumble());
	}

	/// <summary>
	///	Stops rumbling of player's controller
	/// </summary>
	/// <returns>
	/// Wait for rumble duration
	/// </returns>
	public IEnumerator StopRumble()
	{
		yield return new WaitForSeconds(m_rumbleDuration);
		GamePad.SetVibration(m_playerIndex, 0f, 0f);
	}

	/// <summary>
	/// Assigns a controller and player index to a cursor
	/// </summary>
	/// <param name="index">
	/// Represents which controller and player index to set to
	/// </param>
	public void SetController(int index)
	{
		switch (index)
		{
			case 1:
				controller = XboxController.First;
				m_playerIndex = PlayerIndex.One;
				break;
			case 2:
				controller = XboxController.Second;
				m_playerIndex = PlayerIndex.Two;
				break;
			case 3:
				controller = XboxController.Third;
				m_playerIndex = PlayerIndex.Three;
				break;
			default:
				Debug.LogFormat("invalid index: {0} tried to be assigned to {1}", index, gameObject.name);
				break;
		}
	}
}
