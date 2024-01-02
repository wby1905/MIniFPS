using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnInit();
    void OnRecycle();
}

public class ObjectPool<T> where T : MonoBehaviour, IPoolable
{
    private T m_Prefab;
    private Transform m_Parent;
    private Stack<T> m_Pool = new Stack<T>();

    public int Count { get; private set; }

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
        Count = count;
    }

    public T Get()
    {
        T obj;
        if (m_Pool.Count == 0)
        {
            obj = m_Pool.Count > 0 ? m_Pool.Pop() : GameObject.Instantiate(m_Prefab, m_Parent);
            obj.name = $"{m_Prefab.name} {Count}";
            Count++;
        }
        else
        {
            obj = m_Pool.Pop();
        }
        obj.gameObject.SetActive(true);
        obj.OnInit();
        return obj;
    }

    public void Recycle(T obj)
    {
        obj.OnRecycle();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(m_Parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        m_Pool.Push(obj);
    }

}