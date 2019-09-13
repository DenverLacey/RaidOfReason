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
    private const int m_defaultPoolSize = 20;

    public static bool expandWhenNecessary = true;

    public static Dictionary<string, List<GameObject>> objectPools = new Dictionary<string, List<GameObject>>();

    #region Object Pool Checks

    // determines if a pool for a certain prefab has been created
    private static bool PoolExistsForPrefab(string prefabPath)
    {
        return objectPools.ContainsKey(prefabPath);
    }

    // Determines if we can reuse a gameObject for retrieval based on an availability comparator
    private static bool IsAvailableForReuse(GameObject gameObject)
    {
        return !gameObject.activeSelf;
    }

    #endregion

    #region Object Retrieval

    public static GameObject GetPooledObject(string prefabPath, int poolSize = m_defaultPoolSize)
    {
        if (!PoolExistsForPrefab(prefabPath))
            return CreateAndRetrieveFromPool(prefabPath, poolSize);

        var pool = objectPools[prefabPath];

        GameObject instance;

        // Pick the next inactive object.            
        return (instance = FindFirstAvailablePooledObject(pool)) != null ?
            instance : ExpandPoolAndGetObject(prefabPath, pool);
    }

    private static GameObject CreateAndRetrieveFromPool(string prefabPath, int poolSize = m_defaultPoolSize)
    {
        CreateObjectPool(prefabPath, poolSize);
        return GetPooledObject(prefabPath);
    }

    private static GameObject FindFirstAvailablePooledObject(List<GameObject> pool)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (IsAvailableForReuse(pool[i]))
            {
                return pool[i];
            }
        }

        return null;
    }

    #endregion

    #region Object Pool Creation

    private static GameObject ExpandPoolAndGetObject(string prefabPath, List<GameObject> pool)
    {
        if (!expandWhenNecessary) return null;

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        GameObject instance = Object.Instantiate(prefab) as GameObject;
        pool.Add(instance);
        return instance;
    }

    public static List<GameObject> CreateObjectPool(string prefabPath, int count)
    {
        if (count <= 0) count = 1;

        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject instance = Object.Instantiate<GameObject>(prefab);

            objects.Add(instance);

            instance.SetActive(false);
        }

        objectPools.Add(prefabPath, objects);

        return objects;
    }

    #endregion
}