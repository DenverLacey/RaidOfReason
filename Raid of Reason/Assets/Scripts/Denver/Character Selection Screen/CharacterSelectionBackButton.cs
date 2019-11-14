using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using XboxCtrlrInput;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterSelectionBackButton : InteractableUIElement
{
	[Tooltip("How fast the button's border will fade in and out")]
	[SerializeField]
	private float m_flashSpeed = 0.2f;

	[Tooltip("The alpha level at the end of the tween")]
	[SerializeField]
	private float m_finalAlpha = 0.1f;

	private BoxCollider2D m_collider;
	private Image m_borderImage;

	private CharacterSelection m_characterSelection;

	private bool m_flashing;

    // Start is called before the first frame update
    void Start()
    {
		m_characterSelection = FindObjectOfType<CharacterSelection>();
		m_collider = GetComponent<BoxCollider2D>();
		m_borderImage = transform.Find("Button Border").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_characterSelection.PlayerCursors.Exists(cursor => IsCursorHovering(cursor, m_collider)))
		{
			if (m_flashing == false)
			{
				m_borderImage.DOFade(m_finalAlpha, m_flashSpeed).SetLoops(-1, LoopType.Yoyo);
				m_flashing = true;
			}

			if (Utility.IsButtonDownByAnyController(XboxButton.A))
			{
				OnPressed();
			}
		}
		else if(m_flashing == true)
		{
			m_borderImage.DOKill();

			Color fullColor = m_borderImage.color;
			fullColor.a = 1;
			m_borderImage.color = fullColor;

			m_flashing = false;
		}
    }

	private void OnPressed()
	{
		LevelManager.FadeLoadLastLevel();
	}
}
