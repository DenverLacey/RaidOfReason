// Summary: This script handles all the audio clips in the game in either 2d or 3d. You can add as many audio clips to the array.
// This script will not interrupt any audio played from scene to scene.
// Author: Elisha Anagnostakis
// Date: 18/06/2019

using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SoundData[] sound;
    public static AudioManager instance;

    /// <summary>
    /// - When the game boots up this will check if theres only one instance of this object.
    /// - Makes sure the object this script is attached to doesnt get destroyed when changing scenes.
    /// - Adds audio source to the sounds and functionality for the user to tweak with.
    /// </summary>
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(SoundData s in sound)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatial_Blend;
        }
    }

    /// <summary>
    /// - Plays audio in the background.
    /// </summary>
    public void Start()
    {
    }

    /// <summary>
    /// - This function plays the sound the user wants.
    /// - If user creates a typo in the name of the sound when trying 
    ///   to reference it this script will throw you and error.
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
       SoundData s = Array.Find(sound, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound with this " + name + " wasn't found!");
            return;
        }
        s.source.Play();      
    }

    /// <summary>
    /// - Stops playing individual sound.
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        SoundData s = Array.Find(sound, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound with this " + name + " wasn't found!");
            return;
        }
        s.source.Stop();
    }

    /// <summary>
    /// - Stops all sound played in the scene.
    /// </summary>
    public void StopAllSound()
    {
        foreach(SoundData s in sound)
        {
            s.source.Stop();
        }  
    }
}