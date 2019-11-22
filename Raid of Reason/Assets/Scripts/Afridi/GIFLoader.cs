using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIFLoader : MonoBehaviour
{
    public Sprite[] textures;
    public Image image;
    public int speedOfPlay;

    private void Update()
    {
        if (image != null && textures.Length > 0)
        {
            int index = Mathf.RoundToInt(Time.time * speedOfPlay);
            index = index % textures.Length;
            if (index != textures.Length)
            {
                image.sprite = textures[index];
            }
            else
            {
                index = 0;
            }
        }
    }
}

