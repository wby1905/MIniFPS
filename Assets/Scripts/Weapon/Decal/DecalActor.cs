using UnityEngine;

public class DecalActor : ActorController, IPoolable
{
    public ObjectPool<DecalActor> DecalPool { get; set; }

    private float m_LifeTime;

    private float m_Timer = 0f;

    private MeshRenderer m_MeshRenderer;

    public void Spawn(Material mat, Vector3 position, Quaternion rotation)
    {
        if (!HasInit) return;
        transform.position = position;
        transform.rotation = rotation;
        if (m_MeshRenderer != null)
            m_MeshRenderer.material = mat;
    }

    public override void Init(ActorBehaviour mono)
    {
        DecalBehaviour decal = mono as DecalBehaviour;
        if (decal == null) return;
        base.Init(mono);
        m_LifeTime = decal.LifeTime;
    }

    protected override void Awake()
    {
        base.Awake();
        m_Timer = 0f;
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected override void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= m_LifeTime)
        {
            Recycle();
        }

    }

    public void Recycle()
    {
        if (DecalPool != null)
            DecalPool.Recycle(this);
        else
            Destroy(this);
    }

    public void OnInit()
    {
        m_Timer = 0f;
    }

    public void OnRecycle()
    {
        m_Timer = 0f;
        m_MeshRenderer.material = null;
    }
}