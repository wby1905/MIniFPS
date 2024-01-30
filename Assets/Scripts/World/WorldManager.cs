using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WorldManager : Singleton<WorldManager>
{
    public UnityAction OnUpdate;
    public UnityAction OnFixedUpdate;
    public UnityAction OnLateUpdate;
    public UnityAction OnDestroyAction;

    private List<ActorController> m_Actors = new List<ActorController>();
    public PlayerController playerController { get; private set; }

    public PlayerBehaviour playerPrefab;

    public void Register(ActorController actor)
    {
        if (actor == null) return;
        m_Actors.Add(actor);

    }

    public void UnRegister(ActorController actor)
    {
        if (actor == null) return;
        m_Actors.Remove(actor);
    }

    protected override void Awake()
    {
        base.Awake();

        if (playerPrefab != null)
            playerController = Instantiate(playerPrefab, null, true).GetController<PlayerController>();
        else
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                InitActor(player);
                InvokeActor(player);
                playerController = player.GetController<PlayerController>();
            }
        }
    }

    void Update()
    {
        if (OnUpdate != null)
            OnUpdate.Invoke();
    }

    void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate.Invoke();
    }

    void LateUpdate()
    {
        if (OnLateUpdate != null)
            OnLateUpdate.Invoke();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (OnDestroyAction != null)
            OnDestroyAction.Invoke();
        ActorController.worldManager = null;
    }

    // 根据mono prefab实例化
    public static ActorBehaviour Instantiate(ActorBehaviour prefab, Transform parent, bool isActive)
    {
        if (prefab == null) return null;
        var actor = GameObject.Instantiate(prefab, parent);
        actor.gameObject.SetActive(isActive);

        InitActor(actor);
        InvokeActor(actor);
        return actor;
    }

    public static ActorBehaviour Instantiate(ActorData data, Transform parent, bool isActive)
    {
        if (data == null) return null;
        ActorBehaviour actor;
        GameObject go;
        if (data.prefab != null)
        {
            go = GameObject.Instantiate(data.prefab, parent);

        }
        else
        {
            go = new GameObject(data.name);
            go.transform.SetParent(parent);
        }

        actor = go.AddComponent<ActorBehaviour>();
        actor.gameObject.SetActive(isActive);
        InitActor(actor, data);
        InvokeActor(actor);
        return actor;
    }

    public static void InitActor(ActorBehaviour actor, ActorData data = null)
    {
        if (actor == null) return;
        if (data != null)
        {
            actor.Init(data);
        }

        ActorBehaviour[] children = actor.GetComponentsInChildren<ActorBehaviour>();
        foreach (var child in children)
        {
            if (child.transform == actor.transform) continue;
            InitActor(child, data);
        }

        // 根据配置的controller名字添加controller
        if (actor.controllerNames.Count > 0)
        {
            foreach (string name in actor.controllerNames)
            {
                var ac = actor.AddController(name);
                if (data != null)
                {
                    ac.ConfigData(data);
                }
            }
        }
    }

    public static void InvokeActor(ActorBehaviour actor)
    {
        if (actor == null) return;

        foreach (var controller in actor.controllers)
        {
            controller.Invoke();
        }

        ActorBehaviour[] children = actor.GetComponentsInChildren<ActorBehaviour>();
        foreach (var child in children)
        {
            if (child.transform == actor.transform) continue;
            InvokeActor(child);
        }
    }

}