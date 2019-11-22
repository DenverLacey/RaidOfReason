using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum MusicType
{
	TITLE,
	LVL_1,
	LVL_2_PHASE_1,
	LVL_2_PHASE_2,
}

public class MusicManager : MonoBehaviour
{

	[Tooltip("Title Screen Music")]
	[SerializeField]
	private SoundData m_titleScreenMusic;

	[Tooltip("Level 1 Music")]
	[SerializeField]
	private SoundData m_level1Music;

	[Tooltip("Level 2, Phase 1 Music")]
	[SerializeField]
	private SoundData m_level2Phase1Music;

	[Tooltip("Level 2, Phase 2 Music")]
	[SerializeField]
	private SoundData m_level2Phase2Music;

	[Tooltip("Duration of Music Transitions")]
	[SerializeField]
	private float m_transitionDuration;

	private static MusicManager ms_instance;
	private MusicType m_state;
	private SoundData m_currentMusic;
	private SoundData m_nextMusic;

	private Dictionary<MusicType, SoundData> m_tracks = new Dictionary<MusicType, SoundData>();

	private bool m_transition;
	private float m_timer;

	private void Awake()
	{
		if (ms_instance == null)
		{
			ms_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		m_tracks.Add(MusicType.TITLE, m_titleScreenMusic);
		m_tracks.Add(MusicType.LVL_1, m_level1Music);
		m_tracks.Add(MusicType.LVL_2_PHASE_1, m_level2Phase1Music);
		m_tracks.Add(MusicType.LVL_2_PHASE_2, m_level2Phase2Music);
		
		foreach (var pair in m_tracks)
		{
			pair.Value.source = gameObject.AddComponent<AudioSource>();
			pair.Value.source.clip = pair.Value.clip;
			pair.Value.source.loop = pair.Value.loop;
			pair.Value.source.volume = pair.Value.volume;
			pair.Value.source.pitch = pair.Value.pitch;
			pair.Value.source.spatialBlend = pair.Value.spatialBlend;
		}

		m_currentMusic = m_tracks[MusicType.TITLE];
		m_currentMusic.source.Play();

		// setup automatic transitions
		SceneManager.sceneLoaded += TransitionToSceneMusic;
	}

	private void Update()
	{
		if (m_transition)
		{
			if (m_currentMusic == m_nextMusic)
			{
				m_transition = false;
				m_nextMusic = null;
				return;
			}

			m_timer += Time.deltaTime;

			m_currentMusic.source.volume = m_currentMusic.volume - (m_timer / m_transitionDuration) * m_currentMusic.volume;
			m_nextMusic.source.volume = (m_timer / m_transitionDuration) * m_nextMusic.volume;

			if (m_nextMusic.source.volume - m_nextMusic.volume >= 0f)
			{
				m_currentMusic.source.Stop();
				m_currentMusic = m_nextMusic;
				m_currentMusic.source.volume = m_currentMusic.volume;
				m_nextMusic = null;
				m_timer = 0f;
				m_transition = false;
			}
		}
	}

	public static void Transition(MusicType musicType)
	{
		if (musicType == ms_instance.m_state)
			return;


		ms_instance.m_transition = true;
		ms_instance.m_state = musicType;

		switch (musicType)
		{
			case MusicType.TITLE:
				ms_instance.m_nextMusic = ms_instance.m_titleScreenMusic;
				break;
			case MusicType.LVL_1:
				ms_instance.m_nextMusic = ms_instance.m_level1Music;
				break;
			case MusicType.LVL_2_PHASE_1:
				ms_instance.m_nextMusic = ms_instance.m_level2Phase1Music;
				break;
			case MusicType.LVL_2_PHASE_2:
				ms_instance.m_nextMusic = ms_instance.m_level2Phase2Music;
				break;
		}

		ms_instance.m_nextMusic.source.volume = 0f;
		ms_instance.m_nextMusic.source.Play();
	}

	private void TransitionToSceneMusic(Scene s, LoadSceneMode l)
	{
		switch (s.name)
		{
			case "Level 0":
				Transition(MusicType.LVL_1);
				break;
			case "The level":
				Transition(MusicType.LVL_2_PHASE_1);
				ms_instance.m_level2Phase2Music.source.volume = 0f;
				ms_instance.m_level2Phase2Music.source.Play();
				break;
			case "DynamicMenu":
				Transition(MusicType.TITLE);
				break;
			case "CharacterSelectionScene_002":
				Transition(MusicType.TITLE);
				break;
			case "CreditsScene":
				Transition(MusicType.TITLE);
				break;
		}
	}
}
