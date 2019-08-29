using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using XboxCtrlrInput;

public class PlayerCursor : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How fast the cursor will move")]
	private float m_speed;

	[SerializeField]
	[Tooltip("How quickly the token will move")]
	private float m_tokenSpeed = 0.5f;

	[Tooltip("Token Object")]
	public Transform token;

	[HideInInspector]
	public bool hasToken;

	// [HideInInspector]
	public XboxController controller = XboxController.Any;

	[HideInInspector]
	public Transform currentCharacter;

	public Character selectedCharacter;

	private Transform m_collidedTransform = null;

	// Start is called before the first frame update
	void Start()
    {
		hasToken = true;
	}

    // Update is called once per frame
    void Update()
    {
		// if no controller assigned
		if (controller == XboxController.Any)
		{
			return;
		}

		DoMovement();
		DoTokenAndCharacterSelect();
    }

	/// <summary>
	/// moves cursor
	/// </summary>
	void DoMovement()
	{
		// get input
		float x = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float y = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		// scale input by speed and time
		x *= m_speed * Time.deltaTime;
		y *= m_speed * Time.deltaTime;

		transform.Translate(x, y, 0);
	}

	/// <summary>
	/// moves token with cursor
	/// </summary>
	void DoTokenAndCharacterSelect()
	{
		// select character
		if (XCI.GetButtonDown(XboxButton.A, controller) &&
			m_collidedTransform != null)
		{
			if (currentCharacter.GetComponent<CharacterInformation>().SelectCharacter(ref selectedCharacter))
			{
				hasToken = false;
			}
		}

		// deselect character
		if (XCI.GetButtonDown(XboxButton.B, controller))
		{
			if (currentCharacter.GetComponent<CharacterInformation>().DeselectCharacter())
			{
				hasToken = true;
			}
		}

		if (hasToken)
		{
			// move token
			token.position = Vector3.Lerp(token.position, transform.position, m_tokenSpeed);

			// do selected border stuff
			if (m_collidedTransform)
			{
				// if not current character
				if (m_collidedTransform != currentCharacter)
				{
					// TODO: Set character to raycastCharacter
				}
			}
			else
			{
				// TODO: Set character to null
			}

			if (currentCharacter != null)
			{
				Image characterImage = currentCharacter.Find("selectedBorder").GetComponent<Image>();

				characterImage.DOKill();
				characterImage.color = Color.clear;
			}
		}
	}

	void SetCurrentCharacter(Transform t)
	{
		currentCharacter = t;

		if (t != null)
		{
			// border stuff
			Image characterImage = t.Find("selectedBorder").GetComponent<Image>();

			characterImage.color = Color.white;
			characterImage.DOColor(Color.red, .7f).SetLoops(-1);

			// TODO: Ready character
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		m_collidedTransform = collision.transform;
		Debug.LogFormat("{0} hit {1}", gameObject.name, collision.gameObject.name);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		m_collidedTransform = null;
	}
}
