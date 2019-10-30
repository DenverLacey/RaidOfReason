/*
 * Author: Elisha
 * Description: This script allows us to use a unity event system which gives us the ability to choose which button gameobject gets automatically selected when in 
 *              a different area of the main menu for the player to then navigate the menu using the controller.
 */

using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;

public class SetSelectable : MonoBehaviour
{
    public float timer;
    public void SetCurrentSelectable(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public IEnumerator Timer(GameObject Object, float duration)
    {
        yield return new WaitForSeconds(duration);
        Object.SetActive(false);
    }

    public void TimerToDisappear(GameObject Object)
    {
        StartCoroutine(Timer(Object, timer));
    }
}