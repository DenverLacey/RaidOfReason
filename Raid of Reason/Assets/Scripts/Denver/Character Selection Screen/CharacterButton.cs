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

	public Character Character { get => m_character; }

	[SerializeField]
	[Tooltip("What colour image will go when cursor is hovering")]
	private Color m_hoverColour = Color.gray;

	private CharacterSelection m_characterSelection;

	private List<PlayerCursor> m_hoveringCursors = new List<PlayerCursor>();

	private Image m_image;

	private bool m_locked;

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

		if (!m_locked)
		{
			m_image.color = m_hoverColour;
		}
	}

	public void OnCursorClick(PlayerCursor cursor)
	{
		if (m_characterSelection.SelectCharacter(m_character, cursor))
		{
			Lock();
		}
	}

	public void OnCursorExit(PlayerCursor cursor)
	{
		m_characterSelection.HideInfo(m_character, cursor);

		if (!m_locked)
		{
			m_image.color = Color.white;
		}
	}

	public void Lock()
	{
		m_locked = true;
	}

	public void Unlock()
	{
		m_locked = false;
		m_image.color = Color.white;
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
