using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MultiTargetCamera))]
public class MutilTargetCameraEditor : Editor
{
	private SerializedProperty m_rotateSlerpT;

	private void OnEnable()
	{
		m_rotateSlerpT = serializedObject.FindProperty("m_rotateSlerpT");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		serializedObject.Update();

		if (serializedObject.FindProperty("m_rotate").boolValue)
		{
			GUIContent content = new GUIContent("Rotate Slerp T");
			EditorGUILayout.PropertyField(m_rotateSlerpT, content);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
