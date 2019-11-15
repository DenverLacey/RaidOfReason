using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactActor : MonoBehaviour
{
    public GameObject bigCrystal;
    public GameObject smallCrystal;

    public float rotationSpeedBigCrystal;
    public float rotationSpeedSmallCryst3al;

    public float bobSpeedBigCrystal;
    public float bobSpeedSmallCrystal;

    public float bobDistBigCrystal;
    public float bobDistSmallCrystal;

    private float YPositionBig;
    private float YPositionSmall;

    private void Start()
    {
        YPositionBig = bigCrystal.transform.localPosition.y;
        YPositionSmall = smallCrystal.transform.localPosition.y;
    }

    private void Update()
    {
        // rotation
        bigCrystal.transform.rotation = Quaternion.AngleAxis(rotationSpeedBigCrystal, Vector3.up) * bigCrystal.transform.rotation;
        smallCrystal.transform.rotation = Quaternion.AngleAxis(-rotationSpeedSmallCryst3al, Vector3.up) * smallCrystal.transform.rotation;

        // bobbing
        Vector3 desiredPosBig = bigCrystal.transform.localPosition;
        desiredPosBig.y = Mathf.Sin(bobSpeedBigCrystal * Time.time) * bobDistBigCrystal;
        desiredPosBig.y += YPositionBig;
        bigCrystal.transform.localPosition = desiredPosBig;

        Vector3 desiredPosSmall = smallCrystal.transform.localPosition;
        desiredPosSmall.y = -Mathf.Sin(bobSpeedSmallCrystal * Time.time) * bobDistSmallCrystal;
        desiredPosSmall.y += YPositionSmall;
        smallCrystal.transform.localPosition = desiredPosSmall;
    }
}
