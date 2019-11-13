using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using XInputDotNetPure;
using DG.Tweening;

public class CharacterSelectionStartButton : InteractableUIElement
{
	[Tooltip("How fast the buttons border will fade in and out")]
	[SerializeField]
	private float m_flashSpeed = 0.2f;

	[Tooltip("The alpha level at the end of the tween")]
	[SerializeField]
	private float m_finalAlpha = 0.1f;

	private Image m_borderImage;

	private CharacterSelection m_characterSelection;

	private void Awake()
	{
		m_borderImage = transform.Find("Button Border").GetComponent<Image>();
	}

	private void Start()
	{
		m_characterSelection = FindObjectOfType<CharacterSelection>();
	}

	private void OnEnable()
	{
		m_borderImage.DOFade(m_finalAlpha, m_flashSpeed).SetLoops(-1, LoopType.Yoyo);
	}

	private void OnDisable()
	{
		m_borderImage.DOKill();
		Color desiredColour = m_borderImage.color;
		desiredColour.a = 1f;
		m_borderImage.color = desiredColour;
	}

	/// <summary>
	/// When start button is pressed, check that all players have selected a charater
	/// and if so, load level scene
	/// </summary>
	public void OnPressed()
	{
		transform.DOKill(complete: true);

		// get player cursor information
		var p1Info = m_characterSelection.P1Cursor.GetSelectedCharacter();
		var p2Info = m_characterSelection.P2Cursor.GetSelectedCharacter();
		var p3Info = m_characterSelection.P3Cursor.GetSelectedCharacter();

		var playerInformation = new List<(XboxController, PlayerIndex, CharacterType, bool)>();

		// focus controllers that are plugged in
		if (m_characterSelection.P1Cursor.gameObject.activeSelf)
		{
			playerInformation.Add(p1Info);
		}
		if (m_characterSelection.P2Cursor.gameObject.activeSelf)
		{
			playerInformation.Add(p2Info);
		}
		if (m_characterSelection.P3Cursor.gameObject.activeSelf)
		{
			playerInformation.Add(p3Info);
		}

		// if all players are ready
		if (playerInformation.TrueForAll(info => info.Item4) && playerInformation.Count > 1)
		{
			// send info to game manager
			foreach (var info in playerInformation)
			{
				GameManager.Instance.SetCharacterController(info.Item3, info.Item2, info.Item1);
			}

			// load first level
			LevelManager.FadeLoadLevel(m_characterSelection.FirstLevelIndex);
		}
		else
		{
			// do tweening stuff
			transform.DOPunchPosition(Vector3.right * 3f, .3f, 10, 1);
		}

	}
}
