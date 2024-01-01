using UnityEngine;

public class ProjectileWeapon : Weapon
{

    [SerializeField]
    protected Transform m_Ejection;

    [SerializeField]
    protected Projectile m_ProjectilePrefab;

    [SerializeField]
    protected Casing m_CasingPrefab;

    private ObjectPool<Projectile> m_ProjectilePool;
    private ObjectPool<Casing> m_CasingPool;

    public override void Init()
    {
        base.Init();
        if (m_ProjectilePrefab != null)
            m_ProjectilePool = ObjectPoolManager.Instance.CreateOrGetPool<Projectile>(m_ProjectilePrefab, 10);

        if (m_CasingPrefab != null)
            m_CasingPool = ObjectPoolManager.Instance.CreateOrGetPool<Casing>(m_CasingPrefab, 10);
    }

    public override bool CanFire
    {
        get
        {
            return base.CanFire && m_ProjectilePrefab != null;
        }
    }

    public override void Fire(Vector3 aimPoint)
    {
        base.Fire(aimPoint);

        if (m_ProjectilePool != null)
        {
            Projectile projectile = m_ProjectilePool.Get();
            if (projectile != null)
            {
                projectile.ProjectilePool = m_ProjectilePool;
                projectile.Fire(m_Muzzle.Socket.position, aimPoint - m_Muzzle.transform.position);
            }
        }
    }

    public override void EjectCasing()
    {
        base.EjectCasing();

        if (m_CasingPool != null && m_Ejection != null)
        {
            Casing casing = m_CasingPool.Get();
            if (casing != null)
            {
                casing.CasingPool = m_CasingPool;
                casing.Eject(m_Ejection.position, m_Ejection.forward);
            }
        }
    }
}