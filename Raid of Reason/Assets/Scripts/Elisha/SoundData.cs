using UnityEngine.Audio;
using UnityEngine;

/* 
 * Author: Elisha_Anagnostakis
 * Description: This is the data within the sound array that is attached to the Audio 
 *              Manager in which the user can tweak with in the inspector.
 */

[System.Serializable]
public class SoundData
{
    // Audio clip you want to insert.
    public AudioClip clip;
    // Volume of the clip with a range 0-1.
    [Range(0f, 1f)]
    public float volume = 1f;
    // Pitch of the clip with a range 0.1-3.
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    // Does the clip loop forever?
    public bool loop;
    // Sets how much the clip is affected by 3d.
    [Range(0f, 1f)]
    public float spatialBlend = 0f;

    [HideInInspector]
    public AudioSource source;

	public void InitialseAudioSource(AudioSource audioSource)
	{
		source = audioSource;
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.loop = loop;
		audioSource.spatialBlend = spatialBlend;
	}
}