/*
 * Author: Elisha
 * Description: This Script manages the object pooling system for anything that uses projectiles to decrease data usage 
 * and increase overall performance of the game.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooling Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> pooledDictionary;

    private void Start()
    {
        pooledDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            pooledDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 pos, Quaternion rotation)
    {

        if (!pooledDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("pool " + tag + " doesnt exist.");
            return null;
        }

        GameObject objectToSpawn = pooledDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        objectToSpawn.transform.rotation = rotation;

        pooledDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}