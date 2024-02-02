using UnityEngine;

public class PlayerAnimator : ActorController
{
    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private CameraManager m_CameraManager;

    private float m_LerpSpeed = 5f;
    private float m_LookError = 5f;
    private float m_AimSpeed = 5f;
    public Vector3 AimPoint { get; set; }


    private const string OverlayLayer = "Layer Overlay";
    private int m_OverlayLayer;

    private readonly int m_VelocityHash = Animator.StringToHash("Velocity");
    private readonly int m_IsHolsterHash = Animator.StringToHash("IsHolster");
    private readonly int m_FireStateHash = Animator.StringToHash("Fire");
    private readonly int m_IsGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int m_IsRunningHash = Animator.StringToHash("IsRunning");
    private readonly int m_AimingHash = Animator.StringToHash("Aiming");
    private readonly int m_ReloadStateHash = Animator.StringToHash("Reload");

    /*
    * TPS specific animator parameters
    */
    private readonly int m_VelocityXHash = Animator.StringToHash("VelocityX");
    private readonly int m_VelocityZHash = Animator.StringToHash("VelocityZ");

    private CameraMode m_CurCamMode;
    private float m_CurAimValue = -1f;
    private int m_AimStatus = 0;


    private GameObject m_FPSModel;
    private GameObject m_TPSModel;
    public GameObject CurrentModel => m_CurCamMode == CameraMode.FirstPerson ? m_FPSModel : m_TPSModel;


    public override void Init(ActorBehaviour ab)
    {
        if (!(ab is PlayerBehaviour)) return;
        base.Init(ab);
        PlayerBehaviour pb = ab as PlayerBehaviour;
        m_LerpSpeed = pb.LerpSpeed;
        m_LookError = pb.LookError;
        m_AimSpeed = pb.AimSpeed;
        m_FPSModel = pb.FPSModel;
        m_TPSModel = pb.TPSModel;
    }

    protected override void Awake()
    {
        base.Awake();
        m_Animator = GetComponentInChildren<Animator>();
        m_CharacterController = GetComponent<CharacterController>();

        m_CameraManager = CameraManager.Instance;
        if (m_CameraManager != null)
            m_CameraManager.OnSwitchCam += OnSwitchCam;

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (m_CameraManager != null)
            m_CameraManager.OnSwitchCam -= OnSwitchCam;
    }

    protected override void Update()
    {
        base.Update();
        if (m_AimStatus == 1 && m_CurAimValue < 1f)
        {
            m_CurAimValue += Time.deltaTime * m_AimSpeed;
            m_Animator.SetFloat(m_AimingHash, m_CurAimValue);
            if (m_CurAimValue >= 1f)
                m_AimStatus = 0;
        }
        else if (m_AimStatus == -1 && m_CurAimValue > 0f)
        {
            m_CurAimValue -= Time.deltaTime * m_AimSpeed;
            m_Animator.SetFloat(m_AimingHash, m_CurAimValue);
            if (m_CurAimValue <= 0f)
                m_AimStatus = 0;
        }
    }


    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (m_CharacterController == null || m_Animator == null)
            return;

        m_Animator.SetBool(m_IsGroundedHash, m_CharacterController.isGrounded);

        switch (m_CurCamMode)
        {
            case CameraMode.FirstPerson:
                HandleFPSMovement();
                break;
            case CameraMode.ThirdPerson:
                HandleTPSMovement();
                break;
        }

    }

    private void HandleFPSMovement()
    {
        var velocity = m_CharacterController.velocity;

        m_Animator.SetFloat(m_VelocityHash, velocity.magnitude);

        var fpsTransform = m_FPSModel.transform;
        var targetRot = Quaternion.LookRotation(AimPoint - fpsTransform.position);
        if (Mathf.Abs(Quaternion.Angle(fpsTransform.rotation, targetRot)) > m_LookError)
            fpsTransform.rotation = Quaternion.RotateTowards(fpsTransform.rotation, targetRot, Time.deltaTime * m_LerpSpeed);
    }

    private void HandleTPSMovement()
    {
        var velocity = m_CharacterController.velocity;
        var VelocityX = Vector3.Dot(velocity, transform.right);
        var VelocityZ = Vector3.Dot(velocity, transform.forward);

        m_Animator.SetFloat(m_VelocityHash, velocity.magnitude);
        m_Animator.SetFloat(m_VelocityXHash, VelocityX);
        m_Animator.SetFloat(m_VelocityZHash, VelocityZ);

        var weaponSocket = GetController<PlayerInventory>().WeaponSocket;
        if (weaponSocket != null)
        {
            var targetRot = Quaternion.LookRotation(AimPoint - weaponSocket.position);
            if (Mathf.Abs(Quaternion.Angle(weaponSocket.rotation, targetRot)) > m_LookError)
                weaponSocket.rotation = Quaternion.RotateTowards(weaponSocket.rotation, targetRot, Time.deltaTime * m_LerpSpeed);
        }

    }

    void OnSwitchCam(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.FirstPerson:
                m_FPSModel.SetActive(true);
                m_TPSModel.SetActive(false);
                m_Animator = m_FPSModel.GetComponent<Animator>();
                break;
            case CameraMode.ThirdPerson:
                m_FPSModel.SetActive(false);
                m_TPSModel.SetActive(true);
                m_Animator = m_TPSModel.GetComponent<Animator>();
                break;
        }

        if (m_Animator != null)
        {
            m_OverlayLayer = m_Animator.GetLayerIndex(OverlayLayer);
        }

        m_CurCamMode = mode;
    }

    public void SetHolster(bool isHolster)
    {
        m_Animator.SetBool(m_IsHolsterHash, isHolster);
    }

    public void Fire()
    {
        m_Animator.CrossFade(m_FireStateHash, 0.05f, m_OverlayLayer, 0f);
    }

    public void StartAiming()
    {
        m_AimStatus = 1;
    }

    public void StopAiming()
    {
        m_AimStatus = -1;
    }

    public void SetRunning(bool isRunning)
    {
        m_Animator.SetBool(m_IsRunningHash, isRunning);
    }

    public void Reload()
    {
        m_Animator.CrossFade(m_ReloadStateHash, 0.05f, m_OverlayLayer, 0f);
    }

    // Not called by input handler but player controller to get corrected look input
    public void OnLook(Vector2 look)
    {
        if (m_CurCamMode == CameraMode.FirstPerson)
        {
            m_FPSModel.transform.Rotate(-look.y, 0, 0);
        }

    }
}
