using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private ObjectiveManager m_objectiveManager;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        m_objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    public static CheckPointManager Instance { get; private set; }

    [Header("--Respawn Values--")]

    [SerializeField]
    [Tooltip("How long it takes for the player to respwn.")]
    private float m_spawnDelay;
    private float m_currentSpawnDelay;

    private void Start()
    {
        m_currentSpawnDelay = m_spawnDelay;
    }

    public IEnumerator RespawnToCheckpoint(BaseCharacter character, float y)
    {
		yield return new WaitForSeconds(m_spawnDelay);

		character.SoftActivate();
		character.transform.position =  new Vector3(m_objectiveManager.m_currentObjective.SpawnPoints().transform.position.x, y, m_objectiveManager.m_currentObjective.SpawnPoints().transform.position.z);
    }

    public void InvokeRespawn(BaseCharacter character, float y)
    {
        StartCoroutine(RespawnToCheckpoint(character, y));
    }
}