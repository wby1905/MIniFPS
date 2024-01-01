using System.Collections;
using UnityEngine;

public class RigidBullet : Projectile
{
    [SerializeField]
    protected float m_InitialSpeed = 10f;

    [SerializeField]
    protected float m_LifeTime = 5f;

    [SerializeField]
    protected bool m_UseGravity = true;

    protected Rigidbody m_Rigidbody;
    protected Collider m_Collider;
    protected TrailRenderer m_TrailRenderer;

    private float m_LifeTimer = 0f;

    protected override void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_TrailRenderer = GetComponent<TrailRenderer>();
    }

    protected override void Update()
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

    public override void Fire(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;
        transform.rotation = Quaternion.LookRotation(direction);
        m_Rigidbody.velocity = direction * m_InitialSpeed;
        m_Rigidbody.useGravity = m_UseGravity;
        m_Collider.enabled = true;

        m_LifeTimer = m_LifeTime;
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        Vector3 impactPoint = other.contacts[0].point;
        Vector3 impactNormal = other.contacts[0].normal;
        DecalManager.Instance.SpawnDecal(DecalType.BulletHole,
            impactPoint + impactNormal * 0.01f,
            Quaternion.LookRotation(impactNormal, Random.insideUnitSphere)
         );
        Recycle();
    }

    private void Recycle()
    {
        if (ProjectilePool != null)
            ProjectilePool.Recycle(this);
        else
            Destroy(gameObject);
    }

    public override void OnInit()
    {
        m_Collider.enabled = true;
    }

    public override void OnRecycle()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Collider.enabled = false;
        m_TrailRenderer.Clear();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (m_Rigidbody != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, m_Rigidbody.velocity);
        }
    }
#endif
}