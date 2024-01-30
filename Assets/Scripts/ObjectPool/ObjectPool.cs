using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    void OnInit();
    void OnRecycle();
}

public class ObjectPool<T> where T : ActorController, IPoolable, new()
{
    private ActorBehaviour m_Prefab;
    private Transform m_Parent;
    private Stack<T> m_Pool = new Stack<T>();

    public int Count { get; private set; }

    public void Init(ActorBehaviour prefab, Transform parent, int count)
    {
        m_Prefab = prefab;
        m_Parent = parent;
        for (int i = 0; i < count; i++)
        {
            var ab = WorldManager.Instantiate(m_Prefab, m_Parent, false);
            var controller = ab.GetController<T>();
            if (controller == null)
            {
                controller = ab.AddController<T>();
                controller.Invoke();
            }
            controller.gameObject.name = $"{m_Prefab.name} {i}";
            m_Pool.Push(controller);
        }
        Count = count;
    }

    public T Get()
    {
        T controller;
        if (m_Pool.Count == 0)
        {
            var ab = WorldManager.Instantiate(m_Prefab, m_Parent, false);
            controller = ab.GetController<T>();
            if (controller == null)
            {
                controller = ab.AddController<T>();
                controller.Invoke();
            }
            controller.gameObject.name = $"{m_Prefab.name} {Count}";
            Count++;
        }
        else
        {
            controller = m_Pool.Pop();
            if (controller == null)
                return Get();
        }
        controller.SetActive(true);
        controller.OnInit();
        return controller;
    }

    public void Recycle(T obj)
    {
        obj.OnRecycle();
        obj.SetActive(false);
        obj.transform.SetParent(m_Parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        m_Pool.Push(obj);
    }

}