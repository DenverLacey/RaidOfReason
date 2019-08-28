using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
	public PlayerCursor cursor;
	public GameObject[] infoPanels;

	private void Start()
	{
		foreach (var panel in infoPanels)
		{
			panel.SetActive(false);
		}
	}
}
