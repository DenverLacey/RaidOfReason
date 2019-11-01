using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.Rotate(0, 0, -0.5f);
    }
}
