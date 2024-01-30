using UnityEngine;

public class Projectile : ActorController, IPoolable
{
    protected float m_Damage = 10f;

    public ObjectPool<Projectile> ProjectilePool { get; set; }

    protected override void Update() { }
    public virtual void Fire(Vector3 startPos, Vector3 direction) { }

    public virtual void OnInit() { }

    public virtual void OnRecycle() { }
}