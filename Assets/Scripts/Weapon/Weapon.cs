using UnityEngine;
using UnityEngine.Events;

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
    protected int m_TotalAmmo = 90;

    [SerializeField]
    protected AudioClip[] m_FireSounds;
    [SerializeField]
    protected AudioClip m_FireEmptySound;

    [SerializeField]
    protected AudioClip m_ReloadSound;


    protected Muzzle m_Muzzle;
    private Animator m_Animator;
    private WeaponAnimationEventHandler m_AnimationEventHandler;
    private AudioManager m_AudioManager;
    private readonly int m_FireStateHash = Animator.StringToHash("Fire");
    private readonly int m_ReloadStateHash = Animator.StringToHash("Reload");


    public bool IsAutomatic => m_IsAutomatic;
    public int MaxAmmo => m_MaxAmmo;
    public int CurAmmo
    {
        get { return m_CurAmmo; }
        set
        {
            m_CurAmmo = value;
            if (OnAmmoChanged != null)
                OnAmmoChanged.Invoke(m_CurAmmo, m_TotalAmmo);
        }
    }
    public int TotalAmmo
    {
        get { return m_TotalAmmo; }
        set
        {
            m_TotalAmmo = value;
            if (OnAmmoChanged != null)
                OnAmmoChanged.Invoke(m_CurAmmo, m_TotalAmmo);
        }
    }
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

    virtual public bool CanReload
    {
        get
        {
            return State == WeaponState.Idle && m_CurAmmo < m_MaxAmmo && m_TotalAmmo > 0;
        }
    }

    public UnityAction<int, int> OnAmmoChanged;

    virtual protected void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Muzzle = GetComponentInChildren<Muzzle>();
        m_AnimationEventHandler = GetComponentInChildren<WeaponAnimationEventHandler>();
        m_AudioManager = AudioManager.Instance;

        if (m_AnimationEventHandler != null)
        {
            m_AnimationEventHandler.EjectCasing += EjectCasing;
            m_AnimationEventHandler.ReloadComplete += OnReloadComplete;
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
        OnAmmoChanged = null;
        State = WeaponState.NotReady;
    }


    virtual public WeaponState TryFire(Vector3 aimPoint)
    {
        if (!CanFire)
            return WeaponState.NotReady;

        if (m_CurAmmo <= 0)
        {
            if (m_TotalAmmo > 0)
            {
                return WeaponState.Reloading;
            }
            else
                FireEmpty();
        }
        else
        {
            Fire(aimPoint);
        }
        return WeaponState.Firing;
    }

    virtual public void Fire(Vector3 aimPoint)
    {
        if (m_Muzzle == null)
            return;

        m_FireTimer = m_FireCoolDown;
        CurAmmo--;
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
        m_FireTimer = m_FireCoolDown;
        State = WeaponState.Firing;

        if (m_FireEmptySound != null && m_AudioManager != null)
        {
            m_AudioManager.PlayOneShot(m_FireEmptySound, 0.5f, 1f, transform.position);
        }
    }

    virtual public void TryAiming()
    {

    }

    virtual public void StopAiming()
    {

    }

    virtual public bool TryReload()
    {
        if (!CanReload)
            return false;

        Reload();

        return true;
    }

    virtual public void Reload()
    {
        State = WeaponState.Reloading;
        if (m_Animator != null)
        {
            m_Animator.CrossFade(m_ReloadStateHash, 0.05f, 0, 0f);
        }

        if (m_ReloadSound != null && m_AudioManager != null)
        {
            m_AudioManager.PlayOneShot(m_ReloadSound, 0.5f, 1f, transform.position);
        }
    }

    virtual public void OnReloadComplete()
    {
        State = WeaponState.Idle;
        int ammoToReload = Mathf.Min(m_MaxAmmo - m_CurAmmo, m_TotalAmmo);
        CurAmmo += ammoToReload;
        TotalAmmo -= ammoToReload;
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