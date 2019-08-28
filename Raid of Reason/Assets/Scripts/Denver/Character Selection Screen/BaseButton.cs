using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public abstract class BaseButton : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Colour button will go when being hovered over")]
	protected Color m_hoverColour = Color.gray;

	protected Image m_image;
	protected int m_hovers;

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		IncHovers();
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		DecHovers();
	}

	protected virtual void IncHovers()
	{
		m_hovers++;
		m_image.color = m_hoverColour;
	}

	protected virtual void DecHovers()
	{
		m_hovers--;

		if (m_hovers == 0)
		{
			m_image.color = Color.white;
		}
	}

	public abstract void OnPressed(PlayerCursor cursor);
}
