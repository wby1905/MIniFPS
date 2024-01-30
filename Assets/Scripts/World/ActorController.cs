using UnityEngine;

public abstract class ActorController
{
    public static WorldManager worldManager;
    public ActorBehaviour actorBehaviour;
    public GameObject gameObject => actorBehaviour.gameObject;
    public Transform transform => actorBehaviour.transform;
    public bool HasInit => actorBehaviour != null && gameObject != null;
    private bool bHasStarted = false;

    protected ActorController()
    {
        if (worldManager == null)
            worldManager = WorldManager.Instance;
        if (worldManager == null) return;
        worldManager.Register(this);
        worldManager.OnDestroyAction += OnDestroy;

    }

    public virtual void Init(ActorBehaviour ab)
    {
        actorBehaviour = ab;
        ab.controllers.Add(this);
    }

    public virtual void ConfigData(ActorData data)
    {
    }


    public virtual void Invoke()
    {
        Awake();
        if (gameObject.activeSelf)
        {
            bHasStarted = true;
            OnEnable();
            Start();
        }
    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    protected virtual void OnEnable()
    {
        if (!bHasStarted)
        {
            Start();
            bHasStarted = true;
        }
        if (worldManager == null) return;
        worldManager.OnUpdate += Update;
        worldManager.OnFixedUpdate += FixedUpdate;
        worldManager.OnLateUpdate += LateUpdate;
    }

    protected virtual void OnDisable()
    {
        if (worldManager == null) return;
        worldManager.OnUpdate -= Update;
        worldManager.OnFixedUpdate -= FixedUpdate;
        worldManager.OnLateUpdate -= LateUpdate;
    }

    protected virtual void OnDestroy()
    {
        OnDisable();

        if (worldManager == null) return;
        worldManager.UnRegister(this);
        worldManager.OnDestroyAction -= OnDestroy;

        actorBehaviour.TryDestroySelf();
        actorBehaviour = null;
    }

    public T GetComponent<T>() where T : Component
    {
        if (!HasInit) return default;
        return actorBehaviour.GetComponent<T>();
    }

    public T GetComponentInChildren<T>() where T : Component
    {
        if (!HasInit) return default;
        return actorBehaviour.GetComponentInChildren<T>();
    }

    public T GetController<T>() where T : ActorController
    {
        if (!HasInit) return default;
        return actorBehaviour.GetController<T>();
    }

    public T GetControllerInChildren<T>() where T : ActorController
    {
        if (!HasInit) return default;
        return actorBehaviour.GetControllerInChildren<T>();
    }

    public void SetActive(bool active)
    {
        if (!HasInit) return;
        gameObject.SetActive(active);
    }

    public void OnActiveChange(bool active)
    {
        if (active)
        {
            OnEnable();
        }
        else
        {
            OnDisable();
        }
    }
    public virtual void OnCollisionEnter(Collision other)
    {

    }

    public virtual void OnCollisionStay(Collision other)
    {

    }

    public virtual void OnCollisionExit(Collision other)
    {

    }



    /**
    * static methods
*/
    public static implicit operator bool(ActorController actor)
    {
        return actor != null && actor.HasInit;
    }

    public static void Destroy(ActorController actor)
    {
        if (actor == null) return;
        actor.OnDestroy();
    }
}