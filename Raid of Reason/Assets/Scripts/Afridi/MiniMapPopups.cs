using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapPopups : MonoBehaviour
{
    public Camera miniMap;
    public GameObject Objective;
    public Image Arrow;
    Vector3 centreView;

    void Awake()
    {
        centreView = new Vector3(0.5f, 0.5f, 0.0f);
    }

    void FixedUpdate()
    {
        Vector3 objPosView = miniMap.WorldToViewportPoint(Objective.transform.localPosition);
        if (objPosView.z < 0)
        {
            objPosView *= -1;
        }

        Vector3 dirView = (objPosView - centreView);
        float distView = dirView.magnitude;
        dirView.Normalize();

        if (distView > 0.5f)
        {
            objPosView = centreView + dirView * 0.5f;
        }

        objPosView = miniMap.ViewportToScreenPoint(objPosView);
        Arrow.transform.position = objPosView;
    }
}
