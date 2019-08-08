using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

[RequireComponent(typeof(Image))]
public class CharacterButton : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The Character's information panel prefab")]
	private Character m_character;

	private CharacterSelection m_characterSelection;

	private List<PlayerCursor> m_hoveringCursors = new List<PlayerCursor>();

	private Image m_image;

	private void Start()
	{
		m_characterSelection = FindObjectOfType<CharacterSelection>();
		m_image = GetComponent<Image>();
	}

	private void Update()
	{
		foreach (var cursor in m_hoveringCursors)
		{
			if (XCI.GetButtonDown(XboxButton.A, cursor.controller))
			{
				Debug.LogFormat("{0} controller has pressed 'A' over {1}", cursor.controller, gameObject);
				OnCursorClick(cursor);
			}
		}
	}

	public void OnCursorEnter(PlayerCursor cursor)
	{
		m_characterSelection.ShowInfo(m_character, cursor);
	}

	public void OnCursorClick(PlayerCursor cursor)
	{
		m_characterSelection.SelectCharacter(m_character, cursor);
	}

	public void OnCursorExit(PlayerCursor cursor)
	{
		m_characterSelection.HideInfo(m_character, cursor);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerCursor cursor = collision.GetComponent<PlayerCursor>();
		if (cursor)
		{
			OnCursorEnter(cursor);
			m_hoveringCursors.Add(cursor);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerCursor cursor = collision.GetComponent<PlayerCursor>();
		if (cursor)
		{
			m_hoveringCursors.Remove(cursor);
			OnCursorExit(cursor);
		}
	}
}
