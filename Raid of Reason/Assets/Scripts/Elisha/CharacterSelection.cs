/*
 * Author: Denver
 * Description:	Manages all objects and interactions for character selction screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public enum Character
{
	KENRON,
	NASHORN,
	THEA
}

/// <summary>
/// Handles all objects and interactions for character selection screen
/// </summary>
public class CharacterSelection : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Cursor objects for players")]
	private PlayerCursor[] m_cursors;

	[SerializeField]
	[Tooltip("Where a player's information will be displayed")]
	private PlayerPanel[] m_playerPanels;

	private Dictionary<Character, XboxController> m_characterToControllerMap = new Dictionary<Character, XboxController>();

	private CharacterButton[] m_characterButtons;

	private void Start()
	{
		m_characterButtons = FindObjectsOfType<CharacterButton>();
	}

	private void Update()
	{
		// assign controllers
		if (XCI.IsPluggedIn(XboxController.First))
		{
			m_cursors[0].controller = XboxController.First;
			m_cursors[0].gameObject.SetActive(true);
		}
		else
		{
			m_cursors[0].gameObject.SetActive(false);
		}

		if (XCI.IsPluggedIn(XboxController.Second))
		{
			m_cursors[1].controller = XboxController.Second;
			m_cursors[1].gameObject.SetActive(true);
		}
		else
		{
			m_cursors[1].gameObject.SetActive(false);
		}

		if (XCI.IsPluggedIn(XboxController.Third))
		{
			m_cursors[2].controller = XboxController.Third;
			m_cursors[2].gameObject.SetActive(true);
		}
		else
		{
			m_cursors[2].gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Shows Information Panel for a given player cursor
	/// </summary>
	/// <param name="character">
	/// Character that player wants info about
	/// </param>
	/// <param name="cursor">
	/// Cursor that player is controlling
	/// </param>
	public void ShowInfo(Character character, PlayerCursor cursor)
	{
		m_playerPanels[(int)cursor.controller - 1].infoPanels[(int)character].SetActive(true);
	}

	/// <summary>
	/// Hides Information Panel for a given player cursor
	/// </summary>
	/// <param name="character">
	/// Character that player was reading info about
	/// </param>
	/// <param name="cursor">
	/// Cursor that player is controlling
	/// </param>
	public void HideInfo(Character character, PlayerCursor cursor)
	{
		m_playerPanels[(int)cursor.controller - 1].infoPanels[(int)character].SetActive(false);
	}

	/// <summary>
	/// Selects an available character for a given player
	/// </summary>
	/// <param name="character">
	/// Character that player wants to select
	/// </param>
	/// <param name="cursor">
	/// Cursor that the player is controlling
	/// </param>
	/// <returns>
	/// If character could be selected for given player
	/// </returns>
	public bool SelectCharacter(Character character, PlayerCursor cursor)
	{
		// check if character is available or if controller already controlling a character
		if (m_characterToControllerMap.ContainsKey(character))
		{
			return false;
		}

		// check if controller is already controlling another character
		if (m_characterToControllerMap.ContainsValue(cursor.controller))
		{
			Character match = Character.KENRON;
			foreach (var pair in m_characterToControllerMap)
			{
				if (pair.Value == cursor.controller)
				{
					match = pair.Key;
				}
			}
			m_characterToControllerMap.Remove(match);

			UnlockButtonByCharacter(match);
		}

		// add character controller pair
		m_characterToControllerMap[character] = cursor.controller;

		return true;
	}

	/// <summary>
	/// Deselects a character from a given player
	/// </summary>
	/// <param name="cursor">
	/// Cursor of the player that wants to deselect a character
	/// </param>
	/// <returns>
	/// If character could be deselected
	/// </returns>
	public bool DeselectCharacter(PlayerCursor cursor)
	{
		// check if cursor exists in dictionary
		if (m_characterToControllerMap.ContainsValue(cursor.controller))
		{
			// remove from character controller map
			Character match = Character.KENRON;
			foreach (var pair in m_characterToControllerMap)
			{
				if (pair.Value == cursor.controller)
				{
					match = pair.Key;
					break;
				}
			}
			m_characterToControllerMap.Remove(match);

			UnlockButtonByCharacter(match);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Finalises character selections and moves to scene 1
	/// </summary>
	public void AcceptSelection()
	{
		foreach (var pair in m_characterToControllerMap)
		{
			GameManager.Instance.SetCharacterController(pair.Key, pair.Value);
		}

		// load next scene
		if (SceneManager.sceneCount > 0)
		{
			SceneManager.LoadScene(1);
		}
	}

	/// <summary>
	/// Unlocks a button for a character that was selected by a player
	/// </summary>
	/// <param name="character">
	/// Character whose button needs to unlocked
	/// </param>
	/// <returns>
	/// If button was unlocked
	/// </returns>
	private bool UnlockButtonByCharacter(Character character)
	{
		foreach (var button in m_characterButtons)
		{
			if (button.Character == character)
			{
				button.Unlock();
				return true;
			}
		}
		return false;
	}
}
