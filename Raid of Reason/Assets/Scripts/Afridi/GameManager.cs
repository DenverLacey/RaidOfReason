/*
 * Author: Denver
 * Description: This Script manages the game loop. Controlling the Enemy Spawn rates at thier 
 * specified spawn points. By clearing rooms the players gain points
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// Manages everythings that needs to be carried across scenes
/// </summary>
public class GameManager : MonoBehaviour
{
	private GameManager()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private static GameManager ms_instance = null;
	public static GameManager Instance { get => ms_instance; }

    // All Three Players Within the Game
	public Kenron Kenron { get; private set; }
	public Nashorn Nashorn { get; private set; }
	public Thea Thea { get; private set; }

	public List<BaseCharacter> Players { get; private set; }

	public List<BaseCharacter> AlivePlayers
	{
		get
		{
			return Players.FindAll(player => player.playerState == BaseCharacter.PlayerState.ALIVE);
		}
	}

	public List<BaseCharacter> DownedPlayers
	{
		get
		{
			return Players.FindAll(player => player.playerState == BaseCharacter.PlayerState.REVIVE);
		}
	}

	private XboxController[] m_controllers = new XboxController[]
	{
		XboxController.Any, XboxController.Any, XboxController.Any
	};

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Players = new List<BaseCharacter>();
	}

	/// <summary>
	/// Lets characters give a reference to themselves to the GameManager
	/// </summary>
	/// <param name="character">
	/// The character that is giving the GameManager a reference to itself
	/// </param>
	public void GiveCharacterReference(BaseCharacter character)
	{
		if (character is Kenron)
		{
			Kenron = character as Kenron;
			Players.Add(Kenron);

			if (m_controllers[0] != XboxController.Any)
			{
				Kenron.controller = m_controllers[0];
			}
			//else
			//{
			//	Destroy(character.gameObject);
			//}
		}
		else if (character is Nashorn)
		{
			Nashorn = character as Nashorn;
			Players.Add(Nashorn);

			if (m_controllers[1] != XboxController.Any)
			{
				Nashorn.controller = m_controllers[1];
			}
			//else
			//{
			//	Destroy(character.gameObject);
			//}
		}
		else if (character is Thea)
		{
			Thea = character as Thea;
			Players.Add(Thea);

			if (m_controllers[2] != XboxController.Any)
			{
				Thea.controller = m_controllers[2];
			}
			//else
			//{
			//	Destroy(character.gameObject);
			//}
		}
	}

	/// <summary>
	/// Assigns a controller to a character
	/// </summary>
	/// <param name="character">
	/// Character that controller will control
	/// </param>
	/// <param name="controller">
	/// Controller that will control character
	/// </param>
	public void SetCharacterController(Character character, XboxController controller)
	{
		m_controllers[(int)character] = controller;
	}
}
