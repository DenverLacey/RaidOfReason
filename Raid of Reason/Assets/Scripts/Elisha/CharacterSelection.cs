/*
 * Author: Denver
 * Description:	Manages all objects and interactions for character selction screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	public void ShowInfo(Character character, PlayerCursor cursor)
	{
		m_playerPanels[(int)cursor.controller - 1].infoPanels[(int)character].SetActive(true);
	}

	public void HideInfo(Character character, PlayerCursor cursor)
	{
		m_playerPanels[(int)cursor.controller - 1].infoPanels[(int)character].SetActive(false);
	}

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

	public void AcceptSelection()
	{
		foreach (var pair in m_characterToControllerMap)
		{
			switch (pair.Key)
			{
				case Character.KENRON:
					GameManager.Instance.KenronController = pair.Value;
					break;

				case Character.NASHORN:
					GameManager.Instance.NashornController = pair.Value;
					break;

				case Character.THEA:
					GameManager.Instance.TheaController = pair.Value;
					break;

				default:
					Debug.LogErrorFormat("{0} controller couldn't be assigned to a character. Error Value: {1}", pair.Value, pair.Key);
					break;
			}
		}

		// TODO: Move to next scene
	}

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
