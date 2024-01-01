using UnityEngine;

public abstract class Projectile : MonoBehaviour, IPoolable
{

    [SerializeField]
    protected float m_Damage = 10f;

    public ObjectPool<Projectile> ProjectilePool { get; set; }

    abstract protected void Awake();
    abstract protected void Update();
    abstract public void Fire(Vector3 startPos, Vector3 direction);

    abstract public void OnInit();

    abstract public void OnRecycle();
}