using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterInformation : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Character that information is about")]
	private Character m_character;
	public Character Character { get => m_character; }

	private bool m_selected;

	private void Start()
	{
	}

	public bool SelectCharacter(ref Character character)
	{
		if (!m_selected)
		{
			// set character 
			character = m_character;

			// do tween
			transform.DOPunchPosition(Vector3.down * 3, .3f, 10, 1);

			return true;
		}
		else
		{
			return false;
		}
	}

	public bool DeselectCharacter()
	{
		if (m_selected)
		{
			m_selected = false;
			return true;
		}
		else
		{
			return false;
		}
	}
}
