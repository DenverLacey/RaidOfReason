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
public class CharacterSelection : MonoBehaviour
{
	[Tooltip("Player 1's cursor object")]
	[SerializeField]
	private PlayerCursor m_p1Cursor;
	private Vector3 m_p1InactivePosition;
	public PlayerCursor P1Cursor { get => m_p1Cursor; }

	[Tooltip("Player 2's cursor object")]
	[SerializeField]
	private PlayerCursor m_p2Cursor;
	private Vector3 m_p2InactivePosition;
	public PlayerCursor P2Cursor { get => m_p2Cursor; }

	[Tooltip("Player 3's cursor object")]
	[SerializeField]
	private PlayerCursor m_p3Cursor;
	private Vector3 m_p3InactivePosition;
	public PlayerCursor P3Cursor { get => m_p3Cursor; }

	[SerializeField]
	[Tooltip("Start Button Object")]
	private GameObject m_startButton;

	[SerializeField]
	[Tooltip("Scene Index of first level")]
	private int m_firstLevelIndex;

	public int FirstLevelIndex { get => m_firstLevelIndex; }

	private HashSet<PlayerCursor> m_playerCursors = new HashSet<PlayerCursor>();

	// Start is called before the first frame update
	void Start()
    {
		m_p1Cursor.SetController(1);
		m_p2Cursor.SetController(2);
		m_p3Cursor.SetController(3);
		GameManager.Instance.ResetCharacterSelectionSettings();

		m_startButton.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
		// check plugged in controllers
        if (XCI.IsPluggedIn(m_p1Cursor.controller))
		{
			m_p1Cursor.Activate();
			m_playerCursors.Add(m_p1Cursor);
		}
		else if (m_p1Cursor.gameObject.activeSelf)
		{
			m_p1Cursor.Deactivate();
			m_playerCursors.Remove(m_p1Cursor);
		}

		if (XCI.IsPluggedIn(m_p2Cursor.controller))
		{
			m_p2Cursor.Activate();
			m_playerCursors.Add(m_p2Cursor);
		}
		else if (m_p2Cursor.gameObject.activeSelf)
		{
			m_p2Cursor.Deactivate();
			m_playerCursors.Remove(m_p2Cursor);
		}

		if (XCI.IsPluggedIn(m_p3Cursor.controller))
		{
			m_p3Cursor.Activate();
			m_playerCursors.Add(m_p3Cursor);
		}
		else if (m_p3Cursor.gameObject.activeSelf)
		{
			m_p3Cursor.Deactivate();
			m_playerCursors.Remove(m_p3Cursor);
		}

		bool allPlayersSelectedCharacter = false;
		foreach (PlayerCursor pc in m_playerCursors)
		{
			allPlayersSelectedCharacter |= !pc.HasSelectedCharacter();
		}
		allPlayersSelectedCharacter = !allPlayersSelectedCharacter;

		if (allPlayersSelectedCharacter && m_playerCursors.Count > 1)
		{
			ActivateStartButton();
		}
		else
		{
			DeactivateStartButton();
		}
	}

	private void ActivateStartButton()
	{
		m_startButton.SetActive(true);
	}

	private void DeactivateStartButton()
	{
		m_startButton.SetActive(false);
	}
}
