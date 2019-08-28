using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class CharacterButton : BaseButton
{
	[SerializeField]
	[Tooltip("Character that button is for")]
	private Character m_character;
	public Character Character { get => m_character; }
	private bool m_selected;

	protected override void DecHovers()
	{
		m_hovers--;

		if (m_hovers == 0 && !m_selected)
		{
			m_image.color = Color.white;
		}
	}

	public bool SelectCharacter(ref Character character)
	{
		if (!m_selected)
		{
			m_selected = true;
			character = m_character;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DelselectCharacter()
	{

	}

	public override void OnPressed(PlayerCursor cursor)
	{
		throw new System.NotImplementedException();
	}
}
