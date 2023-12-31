using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }

    public bool IsIdle => CurrentState == PlayerState.Idle;

    private InputHandler m_InputHandler;
    private PlayerInventory m_Inventory;
    private PlayerAnimator m_PlayerAnimator;
    public PlayerAnimationEventHandler FPSAnimationEventHandler, TPSAnimationEventHandler;
    public IKHandler FPSIKHandler, TPSIKHandler;
    private IKHandler m_CurIKHandler;

    /**
    * Raycast
    */
    public LayerMask HitLayerMask;
    public float HitDistance = 1000f;
    private Camera m_Camera;
    private RaycastHit m_HitTarget;


    /**
    * Switch weapon
    */
    private bool m_IsHolster = false;
    private int m_WeaponIdx = 0;
    public float SwitchCoolDown = 0.5f;
    private float m_SwitchTimer = 0f;



    void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
        m_Inventory = GetComponent<PlayerInventory>();
        m_PlayerAnimator = GetComponent<PlayerAnimator>();

        if (m_InputHandler != null)
        {
            m_InputHandler.OnSwitchNext += () => TrySwitchWeapon(m_Inventory.CurrentWeaponIndex + 1);
            m_InputHandler.OnSwitchPrev += () => TrySwitchWeapon(m_Inventory.CurrentWeaponIndex - 1);
        }

        if (FPSAnimationEventHandler != null)
        {
            FPSAnimationEventHandler.OnHolster += OnHolster;
        }

        if (TPSAnimationEventHandler != null)
        {
            TPSAnimationEventHandler.OnHolster += OnHolster;
        }

        CurrentState = PlayerState.Idle;

        var cameraManager = CameraManager.Instance;
        if (cameraManager != null)
        {
            cameraManager.OnSwitchCam += OnSwitchCam;
        }
    }

    void Start()
    {
        var cameraManager = CameraManager.Instance;
        if (cameraManager != null)
            m_Camera = cameraManager.CurrentCamera;
    }

    void Update()
    {
        if (m_SwitchTimer > 0f)
            m_SwitchTimer -= Time.deltaTime;


        if (m_Inventory.IsEquipped)
        {
            RaycastFromCrosshair();
            m_PlayerAnimator.AimPoint = m_HitTarget.point;
        }

    }

    void RaycastFromCrosshair()
    {
        if (m_Camera == null)
            return;

        Ray ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (!Physics.Raycast(ray, out m_HitTarget, HitDistance, HitLayerMask))
        {
            m_HitTarget.point = ray.GetPoint(HitDistance);
        }

    }
    void TrySwitchWeapon(int index)
    {
        if (m_Inventory == null || !IsIdle || m_SwitchTimer > 0f)
            return;

        CurrentState = PlayerState.Switching;
        m_WeaponIdx = index;
        m_IsHolster = true;
        m_PlayerAnimator.SetHolster(true);
        m_SwitchTimer = SwitchCoolDown;
    }

    void OnHolster()
    {
        // Equip finished (gun raise)
        if (!m_IsHolster)
        {
            // Switch weapon
            UpdateLeftHandTarget();
            if (CurrentState != PlayerState.Switching)
                return;
            m_IsHolster = true;
            m_PlayerAnimator.SetHolster(true);
            if (m_CurIKHandler != null)
                m_CurIKHandler.LeftHandTarget = null;
        }
        // Unequip finished (gun lower)
        else
        {
            if (m_Inventory != null)
            {
                m_Inventory.EquipWeapon(m_WeaponIdx);
            }
            UpdateLeftHandTarget();
            CurrentState = PlayerState.Idle;
            m_IsHolster = false;
            m_PlayerAnimator.SetHolster(false);
            m_SwitchTimer = SwitchCoolDown;
        }
    }

    void UpdateLeftHandTarget()
    {
        if (m_Inventory == null || m_CurIKHandler == null)
            return;

        var curWeapon = m_Inventory.CurrentWeapon;
        if (curWeapon != null)
        {
            m_CurIKHandler.LeftHandTarget = curWeapon.LeftHandTarget;
            m_CurIKHandler.LeftHandPole = curWeapon.LeftHandPole;
            m_CurIKHandler.RightHandTarget = curWeapon.RightHandTarget;
            m_CurIKHandler.RightHandPole = curWeapon.RightHandPole;
        }
    }

    void OnSwitchCam(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.FirstPerson:
                m_CurIKHandler = FPSIKHandler;
                break;
            case CameraMode.ThirdPerson:
                m_CurIKHandler = TPSIKHandler;
                break;
        }
    }




#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_HitTarget.point, 0.1f);
    }
#endif

}