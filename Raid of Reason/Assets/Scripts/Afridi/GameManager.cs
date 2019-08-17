using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *  
  * Summary:
  * This Script manages the game loop. Controlling the Enemy Spawn rates at thier 
  * specified spawn points. By clearing rooms the players gain points
*/
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

	private XboxController[] m_controllers = new XboxController[]
	{
		XboxController.Any, XboxController.Any, XboxController.Any
	};

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Players = new List<BaseCharacter>();
	}

	public void GiveCharacterReference<T>(T character) where T : BaseCharacter
	{
		if (character is Kenron)
		{
			Kenron = character as Kenron;
			Players.Add(Kenron);

			if (m_controllers[0] != XboxController.Any)
			{
				Kenron.controller = m_controllers[0];
			}
		}
		else if (character is Nashorn)
		{
			Nashorn = character as Nashorn;
			Players.Add(Nashorn);

			if (m_controllers[1] != XboxController.Any)
			{
				Nashorn.controller = m_controllers[1];
			}
		}
		else if (character is Thea)
		{
			Thea = character as Thea;
			Players.Add(Thea);

			if (m_controllers[2] != XboxController.Any)
			{
				Thea.controller = m_controllers[2];
			}
		}
	}

	public void AddController(Character character, XboxController controller)
	{
		m_controllers[(int)character] = controller;
	}
}
