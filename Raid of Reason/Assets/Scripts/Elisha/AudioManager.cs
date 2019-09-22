using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

/*
* Author: Elisha_Anagnostakis
* Description: This script handles all the audio clips in the game in either 2d or 3d sound. You can add as many audio clips to the array.
*               This script will not interrupt any audio played from scene to scene.
*/

public enum SoundType
{
    //Ambient Sounds
    COMBAT_MUSIC,

    // Kenron sounds
    KENRON_WALK,
    KENRON_ATTACK,
    KENRON_HURT,

    // Kreiger sounds
    Kreiger_WALK,
    Kreiger_ATTACK,
    Kreiger_HURT,

    // Thea sounds
    THEA_WALK,
    THEA_ATTACK,
    THEA_HURT,
}

[System.Serializable]
struct StringSoundPair
{
    public SoundType key;
    public SoundData value;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private List<StringSoundPair> m_soundList = new List<StringSoundPair>();
    private Dictionary<SoundType, SoundData> m_sounds = new Dictionary<SoundType, SoundData>();

    [SerializeField]
    private static AudioManager ms_instance;

	public static AudioManager Instance { get => ms_instance; }

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
        foreach (var pair in m_soundList)
        {
            SoundData newSoundData = new SoundData();
            newSoundData.source = gameObject.AddComponent<AudioSource>();
            newSoundData.source.clip = pair.value.clip;
            newSoundData.source.volume = pair.value.volume;
            newSoundData.source.pitch = pair.value.pitch;
            newSoundData.source.loop = pair.value.loop;
            newSoundData.source.spatialBlend = pair.value.spatialBlend;

            // add to dictionary of souds
            m_sounds.Add(pair.key, newSoundData);
        }
    }

    /// <summary>
    /// Plays audio in the background.
    /// </summary>
    public void Start()
    {
        PlaySound(SoundType.COMBAT_MUSIC);
    }

    /// <summary>
    /// This function plays the sound the user wants.
    /// If user creates a typo in the name of the sound when trying 
    /// to reference it this script will throw you and error.
    /// </summary>
    /// <param name="type"></param>
    public void PlaySound(SoundType type)
    {
        SoundData s = PickSoundOfType(type);
        // If it cant find the name in the array
        if(s == null)
        {
            // Throw error 
            Debug.LogWarning("Sound with this " + type + " wasn't found!");
            return;
        }
        // If name can be found then play audio clip
        s.source.Play();      
    }

	public void PlaySound(SoundData sound)
	{
		// create audio source for sound data
		sound.source = gameObject.AddComponent<AudioSource>();
		sound.source.clip = sound.clip;
		sound.source.volume = sound.volume;
		sound.source.pitch = sound.pitch;
		sound.source.loop = sound.loop;
		sound.source.spatialBlend = sound.spatialBlend;

		// play sound
		sound.source.Play();
	}

    /// <summary>
    /// Stops playing individual sound.
    /// </summary>
    /// <param name="type"></param>
    public void StopSound(SoundType type)
    {
        SoundData s = PickSoundOfType(type);
        // If it cant find the name in the array
        if (s == null)
        {
            // Throw error 
            Debug.LogWarning("Sound with this " + type + " wasn't found!");
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
        foreach(var pair in m_sounds)
        {
            // And stops audio clips.
            pair.Value.source.Stop();
        }  
    }

    SoundData PickSoundOfType(SoundType type)
    {
        // create list of all sounds with matching type
        List<SoundData> matchingSounds = new List<SoundData>();

        foreach (var pair in m_sounds)
        {
            if (pair.Key == type)
            {
                matchingSounds.Add(pair.Value);
            }
        }

        // pick random sound from list
        int randIdx = Random.Range(0, matchingSounds.Count);
        return matchingSounds[randIdx];
    }
}