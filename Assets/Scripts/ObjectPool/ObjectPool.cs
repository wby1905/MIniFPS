using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private T m_Prefab;
    private Transform m_Parent;
    private Stack<T> m_Pool = new Stack<T>();

    public int Count => m_Pool.Count;

    public void Init(T prefab, Transform parent, int count)
    {
        m_Prefab = prefab;
        m_Parent = parent;
        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(m_Prefab, m_Parent);
            obj.gameObject.SetActive(false);
            obj.name = $"{m_Prefab.name} {i}";
            m_Pool.Push(obj);
        }
    }

    public T Get()
    {
        T obj = m_Pool.Count > 0 ? m_Pool.Pop() : GameObject.Instantiate(m_Prefab, m_Parent);
        obj.gameObject.SetActive(true);
        obj.OnInit();
        return obj;
    }

    public void Recycle(T obj)
    {
        obj.OnRecycle();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(m_Parent);
        m_Pool.Push(obj);
    }

}