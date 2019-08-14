using UnityEngine.Audio;
using System;
using UnityEngine;

/*
* Author: Elisha_Anagnostakis
* Description: This script handles all the audio clips in the game in either 2d or 3d sound. You can add as many audio clips to the array.
*               This script will not interrupt any audio played from scene to scene.
*/

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private SoundData[] m_sound;
    [SerializeField]
    private static AudioManager ms_instance;

    /// <summary>
    /// When the game boots up this will check if theres only one instance of this object.
    /// Makes sure the object this script is attached to doesnt get destroyed when changing scenes.
    /// Adds audio source to the sounds and functionality for the user to tweak with.
    /// </summary>
    private void Awake()
    {
        // This allows for one instance only
        if(ms_instance == null)
        {
            ms_instance = this;
        }
        else
        {
            // If there is another instance destroy it.
            Destroy(gameObject);
            return;
        }
        // Doesnt destroy object this script is connected too when changing between scenes.
        DontDestroyOnLoad(gameObject);

        // Gets an audio source component automatically when the user wants to add a new clip 
        // and allows the user to edit specific parts of the component of the clip.
        foreach(SoundData s in m_sound)
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
    /// Plays audio in the background.
    /// </summary>
    public void Start()
    {
        // Put all sounds you want to play in the background.
        PlaySound("CombatMusic");
    }

    /// <summary>
    /// This function plays the sound the user wants.
    /// If user creates a typo in the name of the sound when trying 
    /// to reference it this script will throw you and error.
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
       SoundData s = Array.Find(m_sound, sound => sound.name == name);
        // If it cant find the name in the array
        if(s == null)
        {
            // Throw error 
            Debug.LogWarning("Sound with this " + name + " wasn't found!");
            return;
        }
        // If name can be found then play audio clip
        s.source.Play();      
    }

    /// <summary>
    /// Stops playing individual sound.
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(string name)
    {
        SoundData s = Array.Find(m_sound, sound => sound.name == name);
        // If it cant find the name in the array
        if (s == null)
        {
            // Throw error 
            Debug.LogWarning("Sound with this " + name + " wasn't found!");
            return;
        }
        // If name can be found then stop audio clip
        s.source.Stop();
    }

    /// <summary>
    /// Stops all sound played in the scene.
    /// </summary>
    public void StopAllSound()
    {
        // Checks all arrays that use SoundData
        foreach(SoundData s in m_sound)
        {
            // And stops audio clips.
            s.source.Stop();
        }  
    }
}