using UnityEngine;

public class Casing : ActorController, IPoolable
{
    private float m_LifeTime = 2f;
    private float m_LifeTimer = 0f;

    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    public ObjectPool<Casing> CasingPool { get; set; }

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    protected override void Update()
    {
        base.Update();
        if (m_LifeTimer > 0f)
        {
            m_LifeTimer -= Time.deltaTime;
            if (m_LifeTimer <= 0f)
            {
                Recycle();
            }
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        float volume = Random.Range(0.2f, 0.4f);
        float pitch = Random.Range(0.8f, 1.2f);
        AudioManager.Instance.PlayOneShot(AudioType.Casing, volume, pitch, transform.position);

    }

    public void Eject(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;

        Vector3 rndPoint = Random.insideUnitSphere * 1f;
        Vector3 rndDir = (direction + rndPoint).normalized;

        m_Rigidbody.velocity = rndDir * Random.Range(2.5f, 4.5f);
        m_Collider.enabled = true;

        m_LifeTimer = m_LifeTime;
    }

    private void Recycle()
    {
        if (CasingPool != null)
            CasingPool.Recycle(this);
        else
            Destroy(this);
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