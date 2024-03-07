using UnityEngine;

public class ProjectileWeapon : Weapon
{

    protected Transform m_Ejection;
    protected ProjectileBehaviour m_ProjectilePrefab;
    protected ActorBehaviour m_CasingPrefab;

    private ObjectPool<Projectile> m_ProjectilePool;
    private ObjectPool<Casing> m_CasingPool;

    public override void ConfigData(ActorData data)
    {
        base.ConfigData(data);
        ProjectileWeaponData pwd = data as ProjectileWeaponData;
        if (pwd != null)
        {
            m_Ejection = actorBehaviour.FindChild(pwd.Ejection);
            m_ProjectilePrefab = pwd.ProjectilePrefab;
            m_CasingPrefab = pwd.CasingPrefab;
        }
    }

    public override void InitWeapon()
    {
        base.InitWeapon();
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
                if (Owner != null) Physics.IgnoreCollision(projectile.GetComponent<Collider>(), Owner.GetComponent<Collider>());
                else { Debug.LogWarning("Owner is null"); }
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