/*
 * Author: Denver
 * Description: Holds information that must be preserved across multiple scenes
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

/// <summary>
/// Enum used to differentiate between characters
/// </summary>
public enum CharacterType
{
	KENRON,
	KREIGER,
	THEA
}

/// <summary>
/// Manages everythings that needs to be carried across scenes
/// </summary>
public class GameManager : MonoBehaviour
{
    readonly bool m_isInstance;

	private GameManager()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
            m_isInstance = true;

            SceneManager.sceneUnloaded += ObjectPooling.OnSceneUnloaded;
		}
		else
		{
            m_isInstance = false;
		}
	}

	private static GameManager ms_instance = null;
	public static GameManager Instance { get => ms_instance; }

    // All Three Players Within the Game
	public Kenron Kenron { get; private set; }
	public Kreiger Kreiger { get; private set; }
	public Thea Thea { get; private set; }

	public List<BaseCharacter> Players
    {
        get
        {
			var players = new List<BaseCharacter>();
			if (Kenron)
				players.Add(Kenron);

			if (Kreiger)
				players.Add(Kreiger);

			if (Thea)
				players.Add(Thea);

			return players;
        }
    }

    public List<BaseCharacter> DeadPlayers
    {
        get => Players.FindAll(player => player.playerState == BaseCharacter.PlayerState.DEAD);
    }
    public List<BaseCharacter> AlivePlayers
    {
        get => Players.FindAll(player => player.playerState == BaseCharacter.PlayerState.ALIVE);
    }


    public BaseCharacter FirstPlayer
	{
		get
		{
			if (Kenron.playerIndex == PlayerIndex.One)
				return Kenron;
			else if (Kreiger.playerIndex == PlayerIndex.One)
				return Kreiger;
			else
				return Thea;
		}
	}

	public BaseCharacter SecondPlayer
	{
		get
		{
			if (Kenron.playerIndex == PlayerIndex.Two)
				return Kenron;
			else if (Kreiger.playerIndex == PlayerIndex.Two)
				return Kreiger;
			else
				return Thea;
		}
	}

	public BaseCharacter ThirdPlayer
	{
		get
		{
			if (Kenron.playerIndex == PlayerIndex.Three)
				return Kenron;
			else if (Kreiger.playerIndex == PlayerIndex.Three)
				return Kreiger;
			else
				return Thea;
		}
	}

	private XboxController[] m_controllers = new XboxController[]
	{
		XboxController.Any, XboxController.Any, XboxController.Any
	};

	private PlayerIndex[] m_playerIndices = new PlayerIndex[]
	{
		PlayerIndex.Four, PlayerIndex.Four, PlayerIndex.Four
	};

	private void Awake()
	{
        if (m_isInstance)
        {
            // unfreeze scene
            SceneManager.sceneLoaded += (scene, sceneMode) => Time.timeScale = 1.0f;

			DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

			if (m_controllers[0] != XboxController.Any)
			{
				Kenron.controller = m_controllers[0];
				Kenron.playerIndex = m_playerIndices[0];
			}
		}
		else if (character is Kreiger)
		{
			Kreiger = character as Kreiger;

			if (m_controllers[1] != XboxController.Any)
			{
				Kreiger.controller = m_controllers[1];
				Kreiger.playerIndex = m_playerIndices[1];
			}
		}
		else if (character is Thea)
		{
			Thea = character as Thea;

			if (m_controllers[2] != XboxController.Any)
			{
				Thea.controller = m_controllers[2];
				Thea.playerIndex = m_playerIndices[2];
			}
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
	public void SetCharacterController(CharacterType character, PlayerIndex playerIndex, XboxController controller)
	{
		m_controllers[(int)character] = controller;
		m_playerIndices[(int)character] = playerIndex;
	}
}
