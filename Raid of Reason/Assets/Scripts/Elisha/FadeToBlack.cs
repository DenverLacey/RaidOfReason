/*
 * Author: Elisha
 * Description: The one function in the script is used for the 'Fade_Out' animation event in the animation tab so it fades back 
 *              out when changing scenes.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    /// <summary>
    /// This function is for the 'Fade_Out' animation event in which this calls in the inspector.
    /// Applied it like this so it knows when to fade out in the next scene.
    /// </summary>
    public void FadeComplete()
    {
        LevelManager.LoadLevel(LevelManager.m_levelToLoad);
    }
}