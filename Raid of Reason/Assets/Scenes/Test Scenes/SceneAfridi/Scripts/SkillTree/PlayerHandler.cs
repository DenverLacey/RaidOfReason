using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public BaseCharacter Player;

    [SerializeField]
    private Canvas m_Canvas;
    private bool m_SeeCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (m_Canvas) {
                m_SeeCanvas = !m_SeeCanvas;
                m_Canvas.gameObject.SetActive(m_SeeCanvas);
            }
        }
    }
}
