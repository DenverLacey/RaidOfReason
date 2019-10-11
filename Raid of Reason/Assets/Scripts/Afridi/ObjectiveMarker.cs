using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectiveMarker : MonoBehaviour
{
    public Transform MinimapCam;
    public float MinimapRadius;

    void FixedUpdate()
    { 
        Vector3 centerPosition = MinimapCam.transform.localPosition;
        centerPosition.y -= 0.5f;
        float Distance = Vector3.Distance(transform.position, centerPosition);

        if (Distance > MinimapRadius)
        {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= MinimapRadius / Distance;
            transform.position = centerPosition + fromOriginToObject;
        }
    }
}
