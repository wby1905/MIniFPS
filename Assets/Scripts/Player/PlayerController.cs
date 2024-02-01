using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : ActorController
{
    public PlayerState CurrentState { get; private set; }

    public bool IsIdle => CurrentState == PlayerState.Idle;
    public bool IsSwitching => CurrentState == PlayerState.Switching;
    public bool IsRunning => CurrentState == PlayerState.Running;
    public bool IsAiming => CurrentState == PlayerState.Aiming;
    public bool IsReloading => CurrentState == PlayerState.Reloading;
    public bool IsCasting => CurrentState == PlayerState.CastingSkill;
    public bool IsEquipped => m_Inventory != null && m_Inventory.IsEquipped;
    protected bool CanSwitch => m_Inventory != null && IsIdle && m_SwitchTimer <= 0f;
    protected bool CanFire => IsEquipped && (IsIdle || IsAiming || IsRunning);
    protected bool CanAim => IsEquipped && IsIdle;
    protected bool CanRun => IsIdle;
    protected bool CanReload => IsEquipped && IsIdle;
    protected bool CanSkill => IsIdle;

    private InputHandler m_InputHandler;
    private PlayerInventory m_Inventory;
    private PlayerAnimator m_PlayerAnimator;
    private CharacterController m_CharacterController;
    private CameraManager m_CameraManager;


    /**
    * IK
    */
    private PlayerAnimationEventHandler m_FPSAnimationEventHandler, m_TPSAnimationEventHandler;
    private IKHandler m_FPSIKHandler, m_TPSIKHandler;
    private IKHandler m_CurIKHandler;

    /**
    * Movement
    */
    private float m_GroundSpeed = 5f;
    private float m_RunSpeed = 10f;
    private float m_LookSpeed = 200f;
    private float m_LookPitchLimit = 60f;
    private float m_JumpSpeed = 5f;
    private Vector3 m_Velocity;
    private bool m_IsRunning = false;

    /**
    * Fire
    */
    private Vector3 m_FPSAimOffset = new Vector3(-10f, 0, 0);
    private Vector3 m_FPSAimOffsetTop = new Vector3(0, 0.08f, 0);
    private Vector3 m_FPSAimOffsetBottom = new Vector3(0, -0.1f, 0);
    private bool m_IsFiring = false;
    private bool m_IsAiming = false;
    private Vector3 m_FPSCurAimOffset;

    /**
    * Raycast
    */
    private LayerMask m_HitLayerMask;
    private float m_HitDistance = 1000f;
    private Camera m_Camera;
    private Vector3 m_AimPoint;
    private Vector3 m_ScreenCenter = new Vector3(0.5f, 0.5f, 0f);


    /**
    * Switch weapon
    */
    private bool m_IsHolster = false;
    private int m_WeaponIdx = 0;
    private float m_SwitchCoolDown = 0.5f;
    private float m_SwitchTimer = 0f;
    private Weapon m_CurWeapon;
    public Weapon CurWeapon => m_CurWeapon;


    /**
    * Skill
    */
    private SkillSystem m_SkillSystem;
    private bool m_IsCasting = false;
    private bool m_IsIndicatorOpen = false;
    private int m_SkillIdx = 0;


    /**
    * Events
    */
    public UnityAction<Weapon> OnWeaponSwitched;
    public UnityAction<float> OnSkillDeployed;

    public override void Init(ActorBehaviour ab)
    {
        if (!(ab is PlayerBehaviour)) return;
        base.Init(ab);
        PlayerBehaviour pb = ab as PlayerBehaviour;
        m_GroundSpeed = pb.GroundSpeed;
        m_RunSpeed = pb.RunSpeed;
        m_LookSpeed = pb.LookSpeed;
        m_LookPitchLimit = pb.LookPitchLimit;
        m_JumpSpeed = pb.JumpSpeed;
        m_HitLayerMask = pb.HitLayerMask;
        m_HitDistance = pb.HitDistance;
        m_SwitchCoolDown = pb.SwitchCoolDown;
        m_FPSAimOffset = pb.FPSAimOffset;
        m_FPSAimOffsetTop = pb.FPSAimOffsetTop;
        m_FPSAimOffsetBottom = pb.FPSAimOffsetBottom;
        m_FPSAnimationEventHandler = pb.FPSAnimationEventHandler;
        m_TPSAnimationEventHandler = pb.TPSAnimationEventHandler;
        m_FPSIKHandler = pb.FPSIKHandler;
        m_TPSIKHandler = pb.TPSIKHandler;
    }


    protected override void Awake()
    {
        base.Awake();
        m_InputHandler = GetController<InputHandler>();
        m_Inventory = GetController<PlayerInventory>();
        m_PlayerAnimator = GetController<PlayerAnimator>();
        m_SkillSystem = GetController<SkillSystem>();
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
            m_InputHandler.OnReload += TryReloading;
            m_InputHandler.OnSkillPressed += () => m_IsCasting = true;
            m_InputHandler.OnSkillReleased += () => { StopSkill(); m_IsCasting = false; };
        }

        if (m_FPSAnimationEventHandler != null)
        {
            m_FPSAnimationEventHandler.Holster += OnHolster;
            m_FPSAnimationEventHandler.ReloadComplete += OnReloadComplete;
        }

        if (m_TPSAnimationEventHandler != null)
        {
            m_TPSAnimationEventHandler.Holster += OnHolster;
            m_TPSAnimationEventHandler.ReloadComplete += OnReloadComplete;
        }

        CurrentState = PlayerState.Idle;

        if (m_CameraManager != null)
        {
            m_CameraManager.OnSwitchCam += OnSwitchCam;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (m_InputHandler != null)
        {
            m_InputHandler.OnJump = null;
            m_InputHandler.OnSwitchNext = null;
            m_InputHandler.OnSwitchPrev = null;
            m_InputHandler.OnStartFire = null;
            m_InputHandler.OnStopFire = null;
            m_InputHandler.OnStartAim = null;
            m_InputHandler.OnStopAim = null;
            m_InputHandler.OnStartRun = null;
            m_InputHandler.OnStopRun = null;
            m_InputHandler.OnReload = null;
        }

        if (m_FPSAnimationEventHandler != null)
        {
            m_FPSAnimationEventHandler.Holster -= OnHolster;
            m_FPSAnimationEventHandler.ReloadComplete -= OnReloadComplete;
        }

        if (m_TPSAnimationEventHandler != null)
        {
            m_TPSAnimationEventHandler.Holster -= OnHolster;
            m_TPSAnimationEventHandler.ReloadComplete -= OnReloadComplete;
        }

        if (m_CameraManager != null)
        {
            m_CameraManager.OnSwitchCam -= OnSwitchCam;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (m_CameraManager != null)
            m_Camera = m_CameraManager.CurrentCamera;
    }

    protected override void Update()
    {
        base.Update();
        if (m_SwitchTimer > 0f)
            m_SwitchTimer -= Time.deltaTime;

        if (m_Inventory.IsEquipped)
        {
            RaycastFromCrosshair();
            m_PlayerAnimator.AimPoint = m_AimPoint;
        }

        if (m_IsFiring)
            TryFire();

        // aiming and running 不可同时进行
        if (m_IsAiming)
            TryStartAim();
        else if (m_IsRunning)
            TryRunning();

        if (m_IsCasting)
            TrySkill();

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        HandleLook();
        if (m_CharacterController != null)
        {
            HandleGravity();
            HandleMove();
            m_CharacterController.Move(m_Velocity * Time.deltaTime);
        }
    }

    void RaycastFromCrosshair()
    {
        if (m_Camera == null)
        {
            m_Camera = m_CameraManager.CurrentCamera;
            if (m_Camera == null)
                return;
        }

        Ray ray = m_Camera.ViewportPointToRay(m_ScreenCenter);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, m_HitDistance, m_HitLayerMask))
        {
            m_AimPoint = ray.GetPoint(m_HitDistance);
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

        var speed = m_IsRunning ? m_RunSpeed : m_GroundSpeed;
        m_Velocity = moveDir * speed + m_Velocity.y * Vector3.up;
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

        look *= m_LookSpeed * Time.deltaTime;

        switch (camMode)
        {
            case CameraMode.FirstPerson:
                transform.Rotate(0, look.x, 0);
                var curPitch = curCM.transform.eulerAngles.x;
                if (curPitch > 180f) curPitch -= 360f;
                if (curPitch < -180f) curPitch += 360f;
                var deltaPitch = -look.y;
                curPitch += deltaPitch;
                if (curPitch < -m_LookPitchLimit || curPitch > m_LookPitchLimit)
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
        m_Velocity.y = m_JumpSpeed;
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
        m_SwitchTimer = m_SwitchCoolDown;
    }
    void TryFire()
    {
        if (!CanFire) return;
        if (m_CurWeapon != null)
        {
            var state = m_CurWeapon.TryFire(m_AimPoint);
            if (state == WeaponState.Firing)
            {
                m_PlayerAnimator.Fire();
                if (!m_CurWeapon.IsAutomatic)
                    m_IsFiring = false;
            }
            else if (state == WeaponState.Reloading)
            {
                TryReloading();
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
        if (!IsAiming) return;
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
        if (m_Camera == null)
        {
            m_Camera = m_CameraManager.CurrentCamera;
        }
        var curPitch = m_Camera.transform.eulerAngles.x;
        if (curPitch > 180f) curPitch -= 360f;
        if (curPitch < -180f) curPitch += 360f;
        var fraction = (curPitch + m_LookPitchLimit) / (2 * m_LookPitchLimit);
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
        if (!IsRunning) return;
        m_PlayerAnimator.SetRunning(false);
        CurrentState = PlayerState.Idle;
    }

    void TryReloading()
    {
        if (IsAiming)
            StopAiming();
        if (IsRunning)
            StopRunning();

        if (!CanReload) return;

        if (m_CurWeapon != null)
        {
            if (m_CurWeapon.TryReload())
            {
                CurrentState = PlayerState.Reloading;
                m_PlayerAnimator.Reload();
            }
        }
    }

    void TrySkill()
    {
        if (!CanSkill) return;
        CurrentState = PlayerState.CastingSkill;
        if (m_SkillSystem.StartIndicator(m_SkillIdx))
        {
            m_IsIndicatorOpen = true;
        }
    }

    void StopSkill()
    {
        if (!IsCasting) return;

        if (m_IsIndicatorOpen)
        {
            m_SkillSystem.EndIndicator();
            m_IsIndicatorOpen = false;
        }
        float cd = m_SkillSystem.ReleaseSkill(m_SkillIdx);
        if (OnSkillDeployed != null)
            OnSkillDeployed.Invoke(cd);
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
            m_SwitchTimer = m_SwitchCoolDown;
        }
        m_CurWeapon = m_Inventory.CurrentWeapon;
        if (OnWeaponSwitched != null)
            OnWeaponSwitched.Invoke(m_CurWeapon);
    }

    void OnReloadComplete()
    {
        CurrentState = PlayerState.Idle;
    }


    /*
    * Camera events
    */
    void OnSwitchCam(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.FirstPerson:
                m_CurIKHandler = m_FPSIKHandler;
                break;
            case CameraMode.ThirdPerson:
                m_CurIKHandler = m_TPSIKHandler;
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
        else
        {
            m_CurIKHandler.LeftHandTarget = null;
            m_CurIKHandler.LeftHandPole = null;
            m_CurIKHandler.RightHandTarget = null;
            m_CurIKHandler.RightHandPole = null;
        }
    }

}