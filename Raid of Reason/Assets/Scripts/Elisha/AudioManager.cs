using UnityEngine.Audio;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* Author: Elisha_Anagnostakis
* Description: This script handles all the audio clips in the game in either 2d or 3d sound. You can add as many audio clips to the array.
*               This script will not interrupt any audio played from scene to scene.
*/

public enum SoundType
{
    // Kenron sounds
    KENRON_ATTACK,
	KENRON_COOLDOWN,
	KENRON_SKILL,
	KENRON_DEATH,

    // Kreiger sounds
    KRIEGER_ATTACK,
	KRIEGER_COOLDOWN,
	KRIEGER_SKILL,
	KRIEGER_DEATH,

    // Thea sounds
    THEA_ATTACK,
	THEA_COOLDOWN,
	THEA_SKILL,
	THEA_SKILL_CLOSE,
	THEA_DEATH,

	// Enemy SFX
	MELEE_ATTACK,
	RANGE_ATTACK,
	RANGE_ATTACK_HIT,
	SUICIDE_ATTACK,

	// SFX
	RESPAWN,
	BUTTON_CLICK,
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
    private Dictionary<SoundType, List<SoundData>> m_sounds = new Dictionary<SoundType, List<SoundData>>();

	private List<StringSoundPair> m_activeSounds = new List<StringSoundPair>();

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
			if (m_sounds.ContainsKey(pair.key) == false)
				m_sounds.Add(pair.key, new List<SoundData>());
		}

        foreach (var pair in m_soundList)
        {
			//newSoundData.source = gameObject.AddComponent<AudioSource>();
			//newSoundData.source.clip = pair.value.clip;
			//newSoundData.source.volume = pair.value.volume;
			//newSoundData.source.pitch = pair.value.pitch;
			//newSoundData.source.loop = pair.value.loop;
			//newSoundData.source.spatialBlend = pair.value.spatialBlend;

			pair.value.InitialseAudioSource(gameObject.AddComponent<AudioSource>());

			// add to dictionary of souds
			m_sounds[pair.key].Add(pair.value);
        }
    }

    /// <summary>
    /// Plays audio in the background.
    /// </summary>
    public void Start()
    {
        // PlaySound(SoundType.COMBAT_MUSIC);
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

		if (s.source.isPlaying == false)
			s.source.Play();
		else
			PlaySound(s, true);
    }

	public void PlaySound(SoundData sound, bool copy = false)
	{
		if (copy == false)
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
		else
		{
			SoundData dup = new SoundData();
			dup.source = gameObject.AddComponent<AudioSource>();
			dup.source.clip = sound.clip;
			dup.source.volume = sound.volume;
			dup.source.pitch = sound.pitch;
			dup.source.loop = sound.loop;
			sound.source.spatialBlend = sound.spatialBlend;

			// play sound
			dup.source.Play();
			StartCoroutine(RemoveAudioSource(dup.source));
		}
	}

	IEnumerator RemoveAudioSource(AudioSource source)
	{
		yield return new WaitForSecondsRealtime(source.clip.length + 1);
		Destroy(source);
	}

	public void PlaySound(AudioClip clip, bool loop = false)
	{
		SoundData sound = new SoundData
		{
			clip = clip,
			volume = 1f,
			pitch = 1f,
			loop = loop,
			spatialBlend = 0f
		};
		PlaySound(sound);
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
			foreach (var sound in pair.Value)
			{
				// And stops audio clips.
				sound.source.Stop();
			}
        }  
    }

    SoundData PickSoundOfType(SoundType type)
    {
		List<SoundData> m_soundsOfType = m_sounds[type];
		int randIdx = Random.Range(0, m_soundsOfType.Count);
		return m_sounds[type][randIdx];
    }

	private void RemoveSoundFromActivePool(SoundData sound)
	{
		var found = m_activeSounds.Find(p => p.value == sound);
		m_activeSounds.Remove(found);
	}
}