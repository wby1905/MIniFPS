using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 投射物型技能释放器，selector选择投射方向或目标
// 需要prefab中包含含有projectilebehavior组件的子物体
class ProjectileDeployer : SkillDeployer
{
    private static Projectile m_Projectile;
    private static ObjectPool<Projectile> m_ProjectilePool;

    protected override void Start()
    {
        base.Start();
        m_Projectile = actorBehaviour.GetControllerInChildren<Projectile>();
        m_ProjectilePool = ObjectPoolManager.Instance.CreateOrGetPool<Projectile>(m_Projectile.actorBehaviour, 10);
        m_Projectile.SetActive(false);
    }

    public override void DeploySkill()
    {
        CalculateTargets();
        foreach (var pos in SkillData.targetPositions)
        {
            Projectile projectile = m_ProjectilePool.Get();

            projectile.actorBehaviour.onCollisionEnter += OnProjectileEnter;

            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), SkillData.caster.GetComponent<Collider>());
            projectile.ProjectilePool = m_ProjectilePool;
            Debug.DrawLine(transform.position, pos, Color.green, 1);

            // 平行于地面发射
            projectile.Fire(transform.position, pos - transform.position);
        }
    }

    private void OnProjectileEnter(Collision other)
    {
        SkillData.targets = new Transform[] { other.transform };
        ApplyEffects();
    }

    protected override void OnDestroy()
    {
        List<Projectile> projectiles = new List<Projectile>();
        for (int i = 0; i < m_ProjectilePool.Count; i++)
        {
            Projectile p = m_ProjectilePool.Get();
            projectiles.Add(p);
            p.actorBehaviour.onCollisionEnter -= OnProjectileEnter;

            Physics.IgnoreCollision(p.GetComponent<Collider>(), SkillData.caster.GetComponent<Collider>(), false);
        }
        foreach (var p in projectiles)
        {
            m_ProjectilePool.Recycle(p);
        }
        base.OnDestroy();
    }
}