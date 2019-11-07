﻿/*
 * Author: Denver
 * Description:	Utility class used to organise all utility functions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class comprises of all utility functions
/// </summary>
public static class Utility
{
	/// <summary>
	/// Generates a mask to ignore layers
	/// </summary>
	/// <param name="layers">
	/// Name's of the layers that will be ignored
	/// </param>
	/// <returns>
	/// The generated mask
	/// </returns>
	public static int GetIgnoreMask(params string[] layers)
	{
		return ~LayerMask.GetMask(layers);
	}

	/// <summary>
	/// Determines if a given tag is one of the player tags
	/// </summary>
	/// <param name="tag">
	/// possible player tag
	/// </param>
	/// <returns>
	/// if the given tag is a player tag
	/// </returns>
	public static bool TagIsPlayerTag(string tag)
	{
		return tag == "Kenron" || tag == "Thea" || tag == "Kreiger";
	}

	public static bool IsPlayerAvailable(CharacterType characterType)
	{
		switch (characterType)
		{
			case CharacterType.KENRON:
				return GameManager.Instance.Kenron != null && GameManager.Instance.Kenron.playerState == BaseCharacter.PlayerState.ALIVE;

			case CharacterType.KREIGER:
				return GameManager.Instance.Kreiger != null && GameManager.Instance.Kreiger.playerState == BaseCharacter.PlayerState.ALIVE;

			case CharacterType.THEA:
				return GameManager.Instance.Thea != null && GameManager.Instance.Thea.playerState == BaseCharacter.PlayerState.ALIVE;
		}
		return false;
	}
}
