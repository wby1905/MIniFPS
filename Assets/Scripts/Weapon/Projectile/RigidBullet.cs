using System.Collections;
using UnityEngine;

public class RigidBullet : Projectile
{
    protected float m_InitialSpeed = 10f;
    protected float m_LifeTime = 5f;
    protected bool m_UseGravity = true;

    protected Rigidbody m_Rigidbody;
    protected Collider m_Collider;
    protected TrailRenderer m_TrailRenderer;

    private float m_LifeTimer = 0f;

    public override void Init(ActorBehaviour ab)
    {
        RigidBulletBehaviour bb = ab as RigidBulletBehaviour;
        if (bb == null) return;
        base.Init(ab);
        m_Damage = bb.Damage;
        m_InitialSpeed = bb.InitialSpeed;
        m_LifeTime = bb.LifeTime;
        m_UseGravity = bb.UseGravity;
    }

    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_TrailRenderer = GetComponent<TrailRenderer>();
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

    public override void Fire(Vector3 startPos, Vector3 direction)
    {
        transform.position = startPos;
        transform.rotation = Quaternion.LookRotation(direction);
        m_Rigidbody.velocity = direction * m_InitialSpeed;
        m_Rigidbody.useGravity = m_UseGravity;
        m_Collider.enabled = true;

        m_LifeTimer = m_LifeTime;
    }

    public override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        Vector3 impactPoint = other.contacts[0].point;
        Vector3 impactNormal = other.contacts[0].normal;
        DecalManager.Instance.SpawnDecal(DecalType.BulletHole,
            impactPoint + impactNormal * 0.01f,
            Quaternion.LookRotation(impactNormal, Random.insideUnitSphere)
         );

        float volume = Random.Range(0.1f, 0.3f);
        float pitch = Random.Range(0.8f, 1.2f);
        AudioManager.Instance.PlayOneShot(AudioType.BulletImpact, volume, pitch, transform.position);

        Recycle();
    }

    private void Recycle()
    {
        if (ProjectilePool != null)
            ProjectilePool.Recycle(this);
        else
            Destroy(this);
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

}