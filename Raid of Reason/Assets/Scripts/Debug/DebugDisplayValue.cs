using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DebugDisplayValue : MonoBehaviour
{
	#region MakeSingleton
	private static DebugDisplayValue ms_instance = null;
	public static DebugDisplayValue Instance { get => ms_instance; }

	DebugDisplayValue()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
		}
	}
	#endregion	

	private Text m_textField;
	private Dictionary<string, object> m_variables = new Dictionary<string, object>();

	private void Awake()
	{
		m_textField = GetComponent<Text>();
	}

	private void LateUpdate()
	{
		string newText = "";
		// update display
		foreach (var pair in m_variables)
		{
			newText += string.Format("{0}: {1}\n", pair.Key, pair.Value.ToString());
		}
		m_textField.text = newText;
	}

	public void MonitorVariable(string name, object reference)
	{
		m_variables[name] = reference;
	}

	public void StopMonitoringVariable(string name)
	{
		m_variables.Remove(name);
	}
}
