using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    public Transform MinimapCam;
    public float MinimapSize;
    Vector3 TempPos;

    void Update()
    {
        TempPos = transform.parent.transform.position;
        TempPos.y = transform.position.y;
        transform.position = TempPos;
    }

    void LateUpdate()
    {
        Vector3 centerPosition = MinimapCam.transform.position;
        centerPosition.y -= 0.5f;
        float Distance = Vector3.Distance(transform.position, centerPosition);

        if (Distance > MinimapSize)
        {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= MinimapSize / Distance;
            transform.position = centerPosition + fromOriginToObject;
        }
    }
}
