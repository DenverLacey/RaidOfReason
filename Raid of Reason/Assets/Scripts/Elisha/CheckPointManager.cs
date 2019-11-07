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

    public IEnumerator RespawnToCheckpoint(BaseCharacter character)
    {
		yield return new WaitForSeconds(m_spawnDelay);

		character.SoftActivate();
		character.transform.position = m_objectiveManager.m_currentObjective.SpawnPoints().transform.position;

        //foreach (BaseCharacter player in GameManager.Instance.DeadPlayers)
        //{
        //    if (m_objectiveManager.m_currentObjective.name == m_objectiveManager.m_objectives[0].name)
        //    {
        //        player.SoftActivate();
        //        player.gameObject.transform.position = m_objectiveManager.m_currentObjective.SpawnPoints().transform.position;
        //    }
        //}
    }

    public void InvokeRespawn(BaseCharacter character)
    {
		StartCoroutine(RespawnToCheckpoint(character));
    }
}