using System.Collections;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private Animator m_Animator;
    private CharacterController m_CharacterController;

    [SerializeField]
    private float m_LerpSpeed = 5f;
    [SerializeField]
    private float m_LookError = 5f;
    public Vector3 AimPoint { get; set; }


    private int m_IsHolsterHash = Animator.StringToHash("IsHolster");
    private int m_FireStateHash = Animator.StringToHash("Fire");
    private int m_OverlayLayer;
    /*
    * TPS specific animator parameters
    */
    private int m_VelocityXHash = Animator.StringToHash("VelocityX");
    private int m_VelocityZHash = Animator.StringToHash("VelocityZ");
    private int m_IsGroundedHash = Animator.StringToHash("IsGrounded");

    /*
    * FPS specific animator parameters
    */
    private int m_VelocityHash = Animator.StringToHash("Velocity");

    private CameraMode m_CurCamMode;
    public GameObject FPSModel;
    public GameObject TPSModel;
    public GameObject CurrentModel => m_CurCamMode == CameraMode.FirstPerson ? FPSModel : TPSModel;

    void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_CharacterController = GetComponent<CharacterController>();

        var cameraManager = CameraManager.Instance;
        if (cameraManager != null)
            cameraManager.OnSwitchCam += OnSwitchCam;

        var inputHandler = GetComponent<InputHandler>();
    }

    void Start()
    {

    }


    void LateUpdate()
    {
        if (m_CharacterController == null || m_Animator == null)
            return;

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

        var fpsTransform = FPSModel.transform;
        var targetRot = Quaternion.LookRotation(AimPoint - fpsTransform.position);
        if (Mathf.Abs(Quaternion.Angle(fpsTransform.rotation, targetRot)) > m_LookError)
            fpsTransform.rotation = Quaternion.RotateTowards(fpsTransform.rotation, targetRot, Time.deltaTime * m_LerpSpeed);
    }

    private void HandleTPSMovement()
    {
        var velocity = m_CharacterController.velocity;
        var VelocityX = Vector3.Dot(velocity, transform.right);
        var VelocityZ = Vector3.Dot(velocity, transform.forward);

        m_Animator.SetFloat(m_VelocityXHash, VelocityX);
        m_Animator.SetFloat(m_VelocityZHash, VelocityZ);
        m_Animator.SetBool(m_IsGroundedHash, m_CharacterController.isGrounded);

    }

    void OnSwitchCam(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.FirstPerson:
                FPSModel.SetActive(true);
                TPSModel.SetActive(false);
                m_Animator = FPSModel.GetComponent<Animator>();
                break;
            case CameraMode.ThirdPerson:
                FPSModel.SetActive(false);
                TPSModel.SetActive(true);
                m_Animator = TPSModel.GetComponent<Animator>();
                break;
        }

        if (m_Animator != null)
        {
            m_OverlayLayer = m_Animator.GetLayerIndex("Layer Overlay");
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


    // Not called by input handler but player controller to get corrected look input
    public void OnLook(Vector2 look)
    {
        if (m_CurCamMode == CameraMode.FirstPerson)
        {
            FPSModel.transform.Rotate(-look.y, 0, 0);
        }

    }
}
