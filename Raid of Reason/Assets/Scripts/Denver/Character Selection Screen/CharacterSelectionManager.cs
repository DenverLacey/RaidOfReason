using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class CharacterSelectionManager : MonoBehaviour
{
	#region Make Singleton
	private bool m_isInstance;

	private CharacterSelectionManager()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
			m_isInstance = true;
		}
		else
		{
			m_isInstance = false;
		}
	}

	private void Awake()
	{
		if (!m_isInstance)
		{
			Destroy(gameObject);
		}
	}
	#endregion

	private CharacterSelectionManager ms_instance = null;
	public CharacterSelectionManager Instance { get => ms_instance; }

	[SerializeField]
	[Tooltip("All Information Panels")]
	private PlayerPanel[] m_playerPanels;

	private void Update()
	{

	}


}
