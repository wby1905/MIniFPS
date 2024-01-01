using UnityEngine;

public class Casing : MonoBehaviour, IPoolable
{
    [SerializeField]
    private float m_LifeTime = 2f;
    private float m_LifeTimer = 0f;

    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    public ObjectPool<Casing> CasingPool { get; set; }


    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (m_LifeTimer > 0f)
        {
            m_LifeTimer -= Time.deltaTime;
            if (m_LifeTimer <= 0f)
            {
                Recycle();
            }
        }
    }

    public void Eject(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;

        Vector3 rndPoint = Random.insideUnitSphere * 1f;
        Vector3 rndDir = (direction + rndPoint).normalized;

        m_Rigidbody.velocity = rndDir * Random.Range(0.5f, 1.5f);
        m_Collider.enabled = true;

        m_LifeTimer = m_LifeTime;
    }

    private void Recycle()
    {
        if (CasingPool != null)
            CasingPool.Recycle(this);
        else
            Destroy(gameObject);
    }

    public void OnInit()
    {
        m_Collider.enabled = true;
        m_LifeTimer = m_LifeTime;
    }

    public void OnRecycle()
    {
        m_Collider.enabled = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }

}