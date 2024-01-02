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

    [SerializeField]
    protected AudioClip[] m_FireSounds;
    [SerializeField]
    protected AudioClip m_FireEmptySound;


    protected Muzzle m_Muzzle;
    private Animator m_Animator;
    private WeaponAnimationEventHandler m_AnimationEventHandler;
    private AudioManager m_AudioManager;
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

            if (m_IsAutomatic)
                return State == WeaponState.Idle || State == WeaponState.Firing;

            return State == WeaponState.Idle;
        }
    }

    virtual protected void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Muzzle = GetComponentInChildren<Muzzle>();
        m_AnimationEventHandler = GetComponentInChildren<WeaponAnimationEventHandler>();
        m_AudioManager = AudioManager.Instance;

        if (m_AnimationEventHandler != null)
        {
            m_AnimationEventHandler.OnEjectCasing += EjectCasing;
        }
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


    virtual public bool TryFire(Vector3 aimPoint)
    {
        if (!CanFire)
            return false;

        if (m_CurAmmo <= 0)
        {
            FireEmpty();
        }
        else
        {
            Fire(aimPoint);
        }
        return true;
    }

    virtual public void Fire(Vector3 aimPoint)
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

        if (m_FireSounds.Length > 0 && m_AudioManager != null)
        {
            float volume = Random.Range(0.3f, 0.6f);
            float pitch = Random.Range(0.8f, 1.2f);
            AudioClip clip = m_FireSounds[Random.Range(0, m_FireSounds.Length)];
            m_AudioManager.PlayOneShot(clip, volume, pitch, transform.position);
        }
    }

    virtual public void FireEmpty()
    {
        if (m_FireEmptySound != null && m_AudioManager != null)
        {
            m_AudioManager.PlayOneShot(m_FireEmptySound, 0.5f, 1f, transform.position);
        }
    }

    virtual public void EjectCasing()
    {

    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {

    }
#endif
}