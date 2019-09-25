using UnityEngine.EventSystems;
using UnityEngine;

public class SetSelectable : MonoBehaviour
{
    public void SetCurrentSelectable(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}