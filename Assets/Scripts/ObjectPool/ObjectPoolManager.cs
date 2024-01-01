using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<string, object> m_Pools = new Dictionary<string, object>();

    public ObjectPool<T> CreateOrGetPool<T>(T prefab, int count) where T : MonoBehaviour, IPoolable
    {
        string key = prefab.name;
        if (m_Pools.ContainsKey(key))
            return m_Pools[key] as ObjectPool<T>;

        ObjectPool<T> pool = new ObjectPool<T>();
        pool.Init(prefab, transform, count);
        m_Pools.Add(key, pool);
        return pool;
    }

}