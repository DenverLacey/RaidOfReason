/*
 * Author: Elisha
 * Description: This script allows us to use a unity event system which gives us the ability to choose which button gameobject gets automatically selected when in 
 *              a different area of the main menu for the player to then navigate the menu using the controller.
 */

using UnityEngine.EventSystems;
using UnityEngine;

public class SetSelectable : MonoBehaviour
{
    public void SetCurrentSelectable(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}