/*
 * Author: Denver
 * Description:	Is used to tell if an object is an Interactable UI Element
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is used to tell if an object is an Interactable UI Element
/// </summary>
public abstract class InteractableUIElement : MonoBehaviour
{
	protected bool IsCursorHovering(PlayerCursor cursor, BoxCollider2D collider)
	{
		float minx = collider.bounds.min.x;
		float maxx = collider.bounds.max.x;
		float miny = collider.bounds.min.y;
		float maxy = collider.bounds.max.y;

		float cminx = cursor.Pointer.min.x;
		float cmaxx = cursor.Pointer.max.x;
		float cminy = cursor.Pointer.min.y;
		float cmaxy = cursor.Pointer.max.y;

		return cminx <= maxx && cmaxx >= minx &&
				cminy <= maxy && cmaxy >= miny;
	}
}
