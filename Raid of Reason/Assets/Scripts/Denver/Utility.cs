/*
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
		int layerMask = 0;

		foreach (string layerName in layers)
		{
			int layer = LayerMask.NameToLayer(layerName);
			layerMask |= layer;
		}

		return layerMask;
	}
}
