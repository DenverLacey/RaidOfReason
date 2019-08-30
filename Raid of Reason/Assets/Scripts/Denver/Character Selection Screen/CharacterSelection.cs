using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
using DG.Tweening;

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

	private Vector3 m_startButtonPosition;

	[SerializeField]
	[Tooltip("Scene Index of first level")]
	private int m_firstLevelIndex;

	// Start is called before the first frame update
	void Start()
    {
		m_p1Cursor.controller = XboxController.First;
		m_p1InactivePosition = m_p1Cursor.transform.position;

		m_p2Cursor.controller = XboxController.Second;
		m_p2InactivePosition = m_p2Cursor.transform.position;

		m_p3Cursor.controller = XboxController.Third;
		m_p3InactivePosition = m_p3Cursor.transform.position;

		m_startButtonPosition = m_startButton.transform.position;
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
			m_p1Cursor.Deactivate(m_p1InactivePosition);
		}

		if (XCI.IsPluggedIn(m_p2Cursor.controller))
		{
			m_p2Cursor.Activate();
		}
		else if (m_p2Cursor.gameObject.activeSelf)
		{
			m_p2Cursor.Deactivate(m_p2InactivePosition);
		}

		if (XCI.IsPluggedIn(m_p3Cursor.controller))
		{
			m_p3Cursor.Activate();
		}
		else if (m_p3Cursor.gameObject.activeSelf)
		{
			m_p3Cursor.Deactivate(m_p3InactivePosition);
		}
	}

	public void OnPressed()
	{
		transform.DOKill();
		transform.position = m_startButtonPosition;

		// get player cursor information
		var p1Info = m_p1Cursor.GetSelectedCharacter();
		var p2Info = m_p2Cursor.GetSelectedCharacter();
		var p3Info = m_p3Cursor.GetSelectedCharacter();

		List<(XboxController, Character, bool)> playerInformation = new List<(XboxController, Character, bool)>();
		
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
		if (playerInformation.TrueForAll(info => info.Item3))
		{
			// send info to game manager
			foreach (var info in playerInformation)
			{
				GameManager.Instance.SetCharacterController(info.Item2, info.Item1);
			}

			// load first level
			SceneManager.LoadScene(m_firstLevelIndex);
		}
		else
		{
			// do tweening stuff
			transform.DOPunchPosition(Vector3.right * 3f, .3f, 10, 1);
		}
		
	}
}
