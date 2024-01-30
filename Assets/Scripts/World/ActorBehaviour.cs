using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

// ActorController的载体
public class ActorBehaviour : MonoBehaviour
{
    public List<ActorController> controllers = new List<ActorController>();
    public List<string> controllerNames = new List<string>();

    private bool m_Destroyed = false;

    /**
    *  delegates
    */
    public UnityAction<bool> onActiveChange;
    public UnityAction<Collision> onCollisionEnter;
    public UnityAction<Collision> onCollisionStay;
    public UnityAction<Collision> onCollisionExit;


    public virtual void Init(ActorData data)
    {
        controllerNames.AddRange(data.controllerNames);
    }

    public T AddController<T>() where T : ActorController, new()
    {
        T controller = new T();
        controller.Init(this);
        onActiveChange += controller.OnActiveChange;
        onCollisionEnter += controller.OnCollisionEnter;
        onCollisionStay += controller.OnCollisionStay;
        onCollisionExit += controller.OnCollisionExit;
        return controller;
    }

    public ActorController AddController(string name)
    {
        Type t = Type.GetType(name);
        if (t != null && t.IsSubclassOf(typeof(ActorController)))
        {
            MethodInfo method = this.GetType().GetMethod("AddController", new Type[] { }).MakeGenericMethod(t);
            return method.Invoke(this, null) as ActorController;
        }
        return null;
    }

    public T GetController<T>() where T : ActorController
    {
        foreach (var actor in controllers)
        {
            if (actor.GetType() == typeof(T) || actor.GetType().IsSubclassOf(typeof(T)))
                return actor as T;
        }
        return null;
    }

    public T GetControllerInChildren<T>() where T : ActorController
    {
        if (GetController<T>() != null)
            return GetController<T>();

        foreach (var ab in GetComponentsInChildren<ActorBehaviour>())
        {
            if (ab.GetController<T>() != null)
                return ab.GetController<T>();
        }
        return null;
    }

    public Transform FindChild(string name)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(transform);
        while (queue.Count > 0)
        {
            Transform t = queue.Dequeue();
            if (t.name == name)
                return t;
            foreach (Transform child in t)
            {
                queue.Enqueue(child);
            }
        }
        return null;
    }



    void OnEnable()
    {
        if (onActiveChange != null)
            onActiveChange.Invoke(true);
    }

    void OnDisable()
    {
        if (onActiveChange != null)
            onActiveChange.Invoke(false);
    }

    void OnDestroy()
    {
        foreach (var ac in controllers)
        {
            if (ac == null || !ac.HasInit) continue;
            ActorController.Destroy(ac);
        }
        controllers.Clear();
    }

    public void TryDestroySelf()
    {
        if (m_Destroyed) return;
        m_Destroyed = true;
        Destroy(this);
    }



    /**
    * 碰撞相关
    */
    void OnCollisionEnter(Collision other)
    {
        if (onCollisionEnter != null)
            onCollisionEnter.Invoke(other);
    }

    void OnCollisionStay(Collision other)
    {
        if (onCollisionStay != null)
            onCollisionStay.Invoke(other);
    }

    void OnCollisionExit(Collision other)
    {
        if (onCollisionExit != null)
            onCollisionExit.Invoke(other);
    }
}