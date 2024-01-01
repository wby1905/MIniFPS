using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform LeftHandTarget, RightHandTarget;
    public Transform LeftHandPole, RightHandPole;

    public WeaponState State { get; protected set; }
    public WeaponType Type { get; set; }

    [SerializeField]
    protected bool m_IsAutomatic = false;
    [SerializeField]
    protected float m_FireCoolDown = 0.25f;
    protected float m_FireTimer = 0f;


    [SerializeField]
    protected int m_CurAmmo = 30;

    [SerializeField]
    protected int m_MaxAmmo = 30;


    protected Muzzle m_Muzzle;
    private Animator m_Animator;
    private int m_FireStateHash = Animator.StringToHash("Fire");


    public bool IsAutomatic => m_IsAutomatic;
    public int CurAmmo => m_CurAmmo;
    public int MaxAmmo => m_MaxAmmo;
    virtual public bool CanFire
    {
        get
        {
            if (m_FireTimer > 0f)
                return false;

            if (m_CurAmmo <= 0)
                return false;

            if (m_IsAutomatic)
                return State == WeaponState.Idle || State == WeaponState.Firing;

            return State == WeaponState.Idle;
        }
    }

    virtual protected void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Muzzle = GetComponentInChildren<Muzzle>();
    }

    virtual protected void Update()
    {
        if (m_FireTimer > 0f)
            m_FireTimer -= Time.deltaTime;
        else if (State == WeaponState.Firing)
        {
            State = WeaponState.Idle;
        }
    }

    virtual public void Init()
    {
        State = WeaponState.NotReady;
        gameObject.SetActive(false);
    }

    virtual public void OnEquip()
    {
        gameObject.SetActive(true);
        State = WeaponState.Idle;
    }

    virtual public void OnUnequip()
    {
        gameObject.SetActive(false);
        State = WeaponState.NotReady;
    }


    virtual public bool TryFire()
    {
        if (!CanFire)
            return false;

        Fire();
        return true;
    }

    virtual public void Fire()
    {
        if (m_Muzzle == null)
            return;

        m_FireTimer = m_FireCoolDown;
        m_CurAmmo--;
        State = WeaponState.Firing;
        m_Muzzle.Fire();

        if (m_Animator != null)
        {
            m_Animator.CrossFade(m_FireStateHash, 0.05f, 0, 0f);
        }
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {

    }
#endif
}