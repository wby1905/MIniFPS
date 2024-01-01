using UnityEngine;

public class Decal : MonoBehaviour, IPoolable
{
    public ObjectPool<Decal> DecalPool { get; set; }

    [SerializeField]
    private float m_LifeTime = 10f;

    private float m_Timer = 0f;

    private MeshRenderer m_MeshRenderer;

    public void Spawn(Material mat, Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        if (m_MeshRenderer != null)
            m_MeshRenderer.material = mat;
    }

    void Awake()
    {
        m_Timer = 0f;
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    void Update()
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
            Destroy(gameObject);
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