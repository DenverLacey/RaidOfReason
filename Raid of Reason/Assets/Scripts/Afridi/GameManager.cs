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
using UnityEngine.SceneManagement;

/// <summary>
/// Enum used to differentiate between characters
/// </summary>
public enum CharacterType
{
	KENRON,
	NASHORN,
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
	public Nashorn Nashorn { get; private set; }
	public Thea Thea { get; private set; }

	public List<BaseCharacter> Players
    {
        get
        {
            return new List<BaseCharacter>()
            {
                Kenron, Nashorn, Thea
            };
        }
    }

    public Dictionary<CharacterType, List<SkillsAbilities>> CharacterSkillUpgrades { get; private set; }

	public List<BaseCharacter> AlivePlayers
	{
		get
		{
			return Players.FindAll(player => player.playerState == BaseCharacter.PlayerState.ALIVE);
		}
	}

	private XboxController[] m_controllers = new XboxController[]
	{
		XboxController.Any, XboxController.Any, XboxController.Any
	};

	private void Awake()
	{
        if (m_isInstance)
        {
			CharacterSkillUpgrades = new Dictionary<CharacterType, List<SkillsAbilities>>
			{
				{ CharacterType.KENRON, new List<SkillsAbilities>() },
				{ CharacterType.NASHORN, new List<SkillsAbilities>() },
				{ CharacterType.THEA, new List<SkillsAbilities>() }
			};

            // unfreeze scene
            SceneManager.sceneLoaded += (scene, sceneMode) => Time.timeScale = 1.0f;

			DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
	}

    private void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Back, XboxController.Any))
        {
            SceneManager.LoadScene(0);
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
            Kenron.m_skillUpgrades.AddRange(Kenron.GetSkillUpgrades());

			if (m_controllers[0] != XboxController.Any)
			{
				Kenron.controller = m_controllers[0];
			}
		}
		else if (character is Nashorn)
		{
			Nashorn = character as Nashorn;
            Nashorn.m_skillUpgrades.AddRange(Nashorn.GetSkillUpgrades());

			if (m_controllers[1] != XboxController.Any)
			{
				Nashorn.controller = m_controllers[1];
			}
		}
		else if (character is Thea)
		{
			Thea = character as Thea;
            Thea.m_skillUpgrades.AddRange(Thea.GetSkillUpgrades());

			if (m_controllers[2] != XboxController.Any)
			{
				Thea.controller = m_controllers[2];
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
	public void SetCharacterController(CharacterType character, XboxController controller)
	{
		m_controllers[(int)character] = controller;
	}
}

public static class BaseCharacterExtension
{
    public static List<SkillsAbilities> GetSkillUpgrades(this BaseCharacter character)
    {
        if (character is Kenron)
        {
            return GameManager.Instance.CharacterSkillUpgrades[CharacterType.KENRON];
        }
        else if (character is Nashorn)
        {
            return GameManager.Instance.CharacterSkillUpgrades[CharacterType.NASHORN];
        }
        else
        {
            return GameManager.Instance.CharacterSkillUpgrades[CharacterType.THEA];
        }
    }

    public static void AddSkillUpgrade(this BaseCharacter character, SkillsAbilities ability)
    {
        if (character is Kenron)
        {
            GameManager.Instance.CharacterSkillUpgrades[CharacterType.KENRON].Add(ability);
        }
        else if (character is Nashorn)
        {
            GameManager.Instance.CharacterSkillUpgrades[CharacterType.NASHORN].Add(ability);
        }
        else
        {
            GameManager.Instance.CharacterSkillUpgrades[CharacterType.THEA].Add(ability);
        }
    }
}
