using Cinemachine;
using UnityEngine;
using UnityEngine.Video;

public class PlayerController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }

    public bool IsIdle => CurrentState == PlayerState.Idle;
    public bool IsSwitching => CurrentState == PlayerState.Switching;
    public bool IsRunning => CurrentState == PlayerState.Running;
    public bool IsAiming => CurrentState == PlayerState.Aiming;
    public bool CanSwitch => m_Inventory != null && IsIdle && m_SwitchTimer <= 0f;
    public bool CanFire => m_Inventory != null && m_Inventory.IsEquipped && (IsIdle || IsAiming || IsRunning);
    public bool CanAim => m_Inventory != null && m_Inventory.IsEquipped && IsIdle;
    public bool CanRun => IsIdle;

    private InputHandler m_InputHandler;
    private PlayerInventory m_Inventory;
    private PlayerAnimator m_PlayerAnimator;
    private CharacterController m_CharacterController;
    private CameraManager m_CameraManager;


    /**
    * IK
    */
    public PlayerAnimationEventHandler FPSAnimationEventHandler, TPSAnimationEventHandler;
    public IKHandler FPSIKHandler, TPSIKHandler;
    private IKHandler m_CurIKHandler;

    /**
    * Movement
    */
    public float GroundSpeed = 5f;
    public float RunSpeed = 10f;
    public float LookSpeed = 200f;
    public float LookPitchLimit = 60f;
    public float JumpSpeed = 5f;
    private Vector3 m_Velocity;
    private bool m_IsRunning = false;

    /**
    * Fire
    */
    [SerializeField]
    private Vector3 m_FPSAimOffset;
    [SerializeField]
    private Vector3 m_FPSAimOffsetTop = new Vector3(0, 0.08f, 0);
    [SerializeField]
    private Vector3 m_FPSAimOffsetBottom = new Vector3(0, -0.1f, 0);
    private bool m_IsFiring = false;
    private bool m_IsAiming = false;
    private Vector3 m_FPSCurAimOffset;

    /**
    * Raycast
    */
    public LayerMask HitLayerMask;
    public float HitDistance = 1000f;
    private Camera m_Camera;
    private Vector3 m_AimPoint;
    private Vector3 m_ScreenCenter = new Vector3(0.5f, 0.5f, 0f);


    /**
    * Switch weapon
    */
    private bool m_IsHolster = false;
    private int m_WeaponIdx = 0;
    public float SwitchCoolDown = 0.5f;
    private float m_SwitchTimer = 0f;


    private Weapon m_CurWeapon;

    void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
        m_Inventory = GetComponent<PlayerInventory>();
        m_PlayerAnimator = GetComponent<PlayerAnimator>();
        m_CharacterController = GetComponent<CharacterController>();
        m_CameraManager = CameraManager.Instance;

        if (m_InputHandler != null)
        {
            m_InputHandler.OnJump += Jump;
            m_InputHandler.OnSwitchNext += () => TrySwitchWeapon(m_Inventory.CurrentWeaponIndex + 1);
            m_InputHandler.OnSwitchPrev += () => TrySwitchWeapon(m_Inventory.CurrentWeaponIndex - 1);
            m_InputHandler.OnStartFire += () => m_IsFiring = true;
            m_InputHandler.OnStopFire += () => m_IsFiring = false;
            m_InputHandler.OnStartAim += () => m_IsAiming = true;
            m_InputHandler.OnStopAim += () => { StopAiming(); m_IsAiming = false; };
            m_InputHandler.OnStartRun += () => m_IsRunning = true;
            m_InputHandler.OnStopRun += () => { StopRunning(); m_IsRunning = false; };
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

        if (m_CameraManager != null)
        {
            m_CameraManager.OnSwitchCam += OnSwitchCam;
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
            m_PlayerAnimator.AimPoint = m_AimPoint;
        }

        if (m_IsFiring)
            TryFire();

        // aiming and running are mutually exclusive
        if (m_IsAiming)
            TryStartAim();
        else if (m_IsRunning)
            TryRunning();

    }

    void LateUpdate()
    {
        if (m_CharacterController != null)
        {
            HandleGravity();
            HandleLook();
            HandleMove();
            if (IsRunning)
            {
                m_Velocity.x *= RunSpeed;
                m_Velocity.z *= RunSpeed;
            }
            m_CharacterController.Move(m_Velocity * Time.deltaTime);
        }
    }

    void RaycastFromCrosshair()
    {
        if (m_Camera == null)
            return;

        Ray ray = m_Camera.ViewportPointToRay(m_ScreenCenter);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, HitDistance, HitLayerMask))
        {
            m_AimPoint = ray.GetPoint(HitDistance);
        }
        else
        {
            m_AimPoint = hit.point;
        }

    }

    void HandleMove()
    {
        Vector2 movement = m_InputHandler.MoveInput;
        Vector3 moveDir;
        if (m_Camera != null)
            moveDir = m_Camera.transform.forward * movement.y + m_Camera.transform.right * movement.x;
        else
            moveDir = transform.forward * movement.y + transform.right * movement.x;
        moveDir.y = 0;
        moveDir.Normalize();
        m_Velocity = moveDir * GroundSpeed + m_Velocity.y * Vector3.up;
    }

    void HandleGravity()
    {
        if (m_CharacterController.isGrounded && m_Velocity.y < 0)
        {
            // A minor negative velocity to make sure the character is grounded
            m_Velocity.y = -0.05f;
        }
        else
        {
            m_Velocity += Physics.gravity * Time.deltaTime;
        }
    }

    void HandleLook()
    {
        Vector2 look = m_InputHandler.LookInput;
        var curCM = m_CameraManager.CurrentVirtualCamera;
        if (look.sqrMagnitude < 0.01f || curCM == null)
            return;
        var camMode = m_CameraManager.CurrentCameraMode;

        look *= LookSpeed * Time.deltaTime;

        switch (camMode)
        {
            case CameraMode.FirstPerson:
                transform.Rotate(0, look.x, 0);
                var curPitch = curCM.transform.eulerAngles.x;
                if (curPitch > 180f) curPitch -= 360f;
                if (curPitch < -180f) curPitch += 360f;
                var deltaPitch = -look.y;
                curPitch += deltaPitch;
                if (curPitch < -LookPitchLimit || curPitch > LookPitchLimit)
                    return;
                curCM.transform.Rotate(deltaPitch, 0, 0);
                if (IsAiming)
                    AimOffset();
                break;

            case CameraMode.ThirdPerson:
                var pov = curCM.GetCinemachineComponent<CinemachinePOV>();
                if (pov == null) return;
                pov.m_HorizontalAxis.Value += look.x;
                pov.m_VerticalAxis.Value += -look.y;
                break;
        }

        m_PlayerAnimator.OnLook(look);
    }

    /*
    * Input events
    */
    void Jump()
    {
        if (!m_CharacterController.isGrounded)
            return;
        m_Velocity.y = JumpSpeed;
    }

    void TrySwitchWeapon(int index)
    {
        if (IsAiming)
            StopAiming();

        if (IsRunning)
            StopRunning();

        if (!CanSwitch)
            return;

        CurrentState = PlayerState.Switching;
        m_WeaponIdx = index;
        m_IsHolster = true;
        m_PlayerAnimator.SetHolster(true);
        m_SwitchTimer = SwitchCoolDown;
    }
    void TryFire()
    {
        if (!CanFire) return;

        if (m_CurWeapon != null)
        {
            if (m_CurWeapon.TryFire(m_AimPoint))
            {
                m_PlayerAnimator.Fire();
                if (!m_CurWeapon.IsAutomatic)
                    m_IsFiring = false;
            }
        }
    }

    void TryStartAim()
    {
        if (IsRunning)
            StopRunning();

        if (!CanAim) return;

        CurrentState = PlayerState.Aiming;
        m_PlayerAnimator.StartAiming();
        m_CurWeapon.TryAiming();
        AimOffset();
    }

    void StopAiming()
    {
        CurrentState = PlayerState.Idle;
        m_PlayerAnimator.StopAiming();
        m_CurWeapon.StopAiming();

        if (m_CameraManager.CurrentCameraMode == CameraMode.FirstPerson)
        {
            m_CameraManager.ResetCurCamera();
        }
    }

    private void AimOffset()
    {
        // temporary solution for camera offset
        // may be configurable for every weapon
        var curPitch = m_Camera.transform.eulerAngles.x;
        if (curPitch > 180f) curPitch -= 360f;
        if (curPitch < -180f) curPitch += 360f;
        var fraction = (curPitch + LookPitchLimit) / (2 * LookPitchLimit);
        m_FPSCurAimOffset = m_FPSAimOffset +
                            Vector3.Lerp(m_FPSAimOffsetTop, m_FPSAimOffsetBottom, fraction);
        m_CameraManager.MoveCurCamera(m_FPSCurAimOffset);
    }

    void TryRunning()
    {
        if (IsAiming)
            StopAiming();

        if (!CanRun) return;

        m_PlayerAnimator.SetRunning(true);
        CurrentState = PlayerState.Running;
    }

    void StopRunning()
    {
        m_PlayerAnimator.SetRunning(false);
        CurrentState = PlayerState.Idle;
    }


    /*
    * Animation events
    */
    void OnHolster()
    {
        // Equip finished (gun raise)
        if (!m_IsHolster)
        {
            // switch complete
            if (CurrentState == PlayerState.Unequipped)
                CurrentState = PlayerState.Idle;
            UpdateHandIK();
            // Switch weapon
            if (CurrentState == PlayerState.Switching)
            {
                m_IsHolster = true;
                m_PlayerAnimator.SetHolster(true);
                m_Inventory.UnequipWeapon();
                if (m_CurIKHandler != null)
                    m_CurIKHandler.LeftHandTarget = null;
            }
        }
        // Unequip finished (gun lower)
        else
        {
            if (m_Inventory != null)
            {
                m_Inventory.EquipWeapon(m_WeaponIdx);
            }
            UpdateHandIK();
            CurrentState = PlayerState.Unequipped;
            m_IsHolster = false;
            m_PlayerAnimator.SetHolster(false);
            m_SwitchTimer = SwitchCoolDown;
        }

        m_CurWeapon = m_Inventory.CurrentWeapon;
    }


    /*
    * Camera events
    */
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


    /*
    * IK
    */
    private void UpdateHandIK()
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

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_AimPoint, 0.1f);
    }

    void OnGUI()
    {

    }
#endif

}