/*
 * Author: Denver
 * Description:	Handles controllers for Character Selection screen and loading level scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;
using DG.Tweening;

/// <summary>
/// Handles controllers for Character Selection screen and loading level scene
/// </summary>
public class CharacterSelection : InteractableUIElement
{
	[SerializeField]
	[Tooltip("Player 1's cursor object")]
	private PlayerCursor m_p1Cursor;
	private Vector3 m_p1InactivePosition;

	[SerializeField]
	[Tooltip("Player 2's cursor object")]
	private PlayerCursor m_p2Cursor;
	private Vector3 m_p2InactivePosition;

	[SerializeField]
	[Tooltip("Player 3's cursor object")]
	private PlayerCursor m_p3Cursor;
	private Vector3 m_p3InactivePosition;

	[SerializeField]
	[Tooltip("Start Button Object")]
	private GameObject m_startButton;

	[SerializeField]
	[Tooltip("Scene Index of first level")]
	private int m_firstLevelIndex;

	// Start is called before the first frame update
	void Start()
    {
		m_p1Cursor.SetController(1);
		m_p2Cursor.SetController(2);
		m_p3Cursor.SetController(3);
		GameManager.Instance.ResetCharacterSelectionSettings();
	}

	// Update is called once per frame
	void Update()
    {
		// check plugged in controllers
        if (XCI.IsPluggedIn(m_p1Cursor.controller))
		{
			m_p1Cursor.Activate();
		}
		else if (m_p1Cursor.gameObject.activeSelf)
		{
			m_p1Cursor.Deactivate();
		}

		if (XCI.IsPluggedIn(m_p2Cursor.controller))
		{
			m_p2Cursor.Activate();
		}
		else if (m_p2Cursor.gameObject.activeSelf)
		{
			m_p2Cursor.Deactivate();
		}

		if (XCI.IsPluggedIn(m_p3Cursor.controller))
		{
			m_p3Cursor.Activate();
		}
		else if (m_p3Cursor.gameObject.activeSelf)
		{
			m_p3Cursor.Deactivate();
		}
	}

	/// <summary>
	/// When start button is pressed, check that all players have selected a charater
	/// and if so, load level scene
	/// </summary>
	public void OnPressed()
	{
		transform.DOKill(complete: true);

		// get player cursor information
		var p1Info = m_p1Cursor.GetSelectedCharacter();
		var p2Info = m_p2Cursor.GetSelectedCharacter();
		var p3Info = m_p3Cursor.GetSelectedCharacter();

		var playerInformation = new List<(XboxController, PlayerIndex, CharacterType, bool)>();
		
		// focus controllers that are plugged in
		if (m_p1Cursor.gameObject.activeSelf)
		{
			playerInformation.Add(p1Info);
		}
		if (m_p2Cursor.gameObject.activeSelf)
		{
			playerInformation.Add(p2Info);
		}
		if (m_p3Cursor.gameObject.activeSelf)
		{
			playerInformation.Add(p3Info);
		}

		// if all players are ready
		if (playerInformation.TrueForAll(info => info.Item4))
		{
			// send info to game manager
			foreach (var info in playerInformation)
			{
				GameManager.Instance.SetCharacterController(info.Item3, info.Item2, info.Item1);
			}

			// load first level
			LevelManager.FadeLoadLevel(m_firstLevelIndex);
		}
		else
		{
			// do tweening stuff
			transform.DOPunchPosition(Vector3.right * 3f, .3f, 10, 1);
		}
		
	}
}
