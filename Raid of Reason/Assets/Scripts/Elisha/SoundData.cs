// Summary: This is the data within the sound array that the user can tweak with
// Author: Elisha Anagnostakis
// Date: 18/06/2019

using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// This class holds all the data that allows the audio to play.
/// </summary>
[System.Serializable]
public class SoundData
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    [Range(0f, 1f)]
    public float spatial_Blend;

    [HideInInspector]
    public AudioSource source;
}