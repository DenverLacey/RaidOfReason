using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

[RequireComponent(typeof(Image))]
public class StartButton : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Colour button will go when cursor is hovering")]
	private Color m_hoverColour = Color.gray;

	private CharacterSelection m_characterSelection;

	private Image m_image;
	private List<PlayerCursor> m_hoveringCursors = new List<PlayerCursor>();

    // Start is called before the first frame update
    void Start()
    {
		m_image = GetComponent<Image>();
		m_characterSelection = FindObjectOfType<CharacterSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cursor in m_hoveringCursors)
		{
			if (XCI.GetButtonDown(XboxButton.A, cursor.controller))
			{
				m_characterSelection.AcceptSelection();
			}
		}
    }

	private void OnCursorEnter(PlayerCursor cursor)
	{
		m_image.color = m_hoverColour;
	}

	private void OnCursorExit(PlayerCursor cursor)
	{
		if (m_hoveringCursors.Count == 0)
		{
			m_image.color = Color.white;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerCursor cursor = collision.GetComponent<PlayerCursor>();
		if (cursor)
		{
			m_hoveringCursors.Add(cursor);
			OnCursorEnter(cursor);
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
