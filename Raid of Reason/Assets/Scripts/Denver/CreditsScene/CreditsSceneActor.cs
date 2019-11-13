﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using XboxCtrlrInput;

[RequireComponent(typeof(VideoPlayer))]
public class CreditsSceneActor : MonoBehaviour
{
	[Tooltip("How many seconds to chop off at the end")]
	[SerializeField]
	private float m_pausePoint = 2f;

	private VideoPlayer m_videoPlayer;

	private float m_timer;

	private int m_phase;

    // Start is called before the first frame update
    void Start()
    {
		m_videoPlayer = GetComponent<VideoPlayer>();       
    }

    // Update is called once per frame
    void Update()
    {
		if (m_phase == 0)
		{
			m_timer += Time.deltaTime;
		}

		if (m_phase == 0 && m_timer >= m_videoPlayer.length - m_pausePoint)
		{
			m_videoPlayer.Pause();
			m_phase = 1;
		}

		if (m_phase == 1)
		{
			if (Input.GetKeyDown(KeyCode.Return) || XCI.GetButton(XboxButton.Start, XboxController.Any))
			{
				LevelManager.FadeLoadLevel(0);
			}
		}
    }
}
