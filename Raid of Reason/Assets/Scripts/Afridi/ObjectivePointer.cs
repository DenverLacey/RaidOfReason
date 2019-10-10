using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ObjectivePointer : MonoBehaviour
{
    public Transform MinimapCam;
    public float MinimapRadius;
    Vector3 temp;

    void Update()
    {
        temp = transform.parent.transform.position;
        temp.y = transform.position.y;
        transform.position = temp;
    }

    void FixedUpdate()
    {
        // Center of Minimap
        Vector3 centerPosition = MinimapCam.transform.localPosition;

        // Just to keep a distance between Minimap camera and this Object (So that camera don't clip it out)
        centerPosition.y -= 0.5f;

        // Distance from the gameObject to Minimap
        float Distance = Vector3.Distance(transform.position, centerPosition);

        // If the Distance is less than MinimapRadius, it is within the Minimap view and we don't need to do anything
        // But if the Distance is greater than the MinimapRadius, then do this
        if (Distance > MinimapRadius)
        {
            // Gameobject - Minimap
            Vector3 fromOriginToObject = transform.position - centerPosition;

            // Multiply by MinimapRadius and Divide by Distance
            fromOriginToObject *= MinimapRadius / Distance;

            // Minimap + above calculation
            transform.position = centerPosition + fromOriginToObject;
        }
    }
}
