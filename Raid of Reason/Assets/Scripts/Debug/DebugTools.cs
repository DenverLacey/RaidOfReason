using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTools : MonoBehaviour
{
	#region Make Singleton
	public static DebugTools Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null || !Application.isEditor)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}
	#endregion

	[SerializeField]
	private TextMeshProUGUI m_textField;
	private Dictionary<string, object> m_loggedVariables = new Dictionary<string, object>();

	private void LateUpdate()
	{
		string newText = "";
		foreach (var pair in m_loggedVariables)
		{
			newText += string.Format("{0}: {1}\n", pair.Key, (pair.Value != null ? pair.Value.ToString() : "null"));
		}
		m_textField.text = newText;
	}

	private void LogVariableInstance(string name, object obj)
	{
		m_loggedVariables[name] = obj;
	}

	public static void LogVariable(string name, object obj)
	{
		if (Instance)
		{
			Instance.LogVariableInstance(name, obj);
		}
	}
}
