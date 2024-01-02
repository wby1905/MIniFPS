using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<string, object> m_Pools = new Dictionary<string, object>();

    public ObjectPool<T> CreateOrGetPool<T>(T prefab, int count, Transform parent = null) where T : MonoBehaviour, IPoolable
    {
        string key = prefab.name;
        if (m_Pools.ContainsKey(key))
            return m_Pools[key] as ObjectPool<T>;

        ObjectPool<T> pool = new ObjectPool<T>();
        if (parent == null)
            parent = transform;
        pool.Init(prefab, parent, count);
        m_Pools.Add(key, pool);
        return pool;
    }

}