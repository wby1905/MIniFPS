using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private Animator m_Animator;
    private CharacterController m_CharacterController;

    private int m_IsHolsterHash = Animator.StringToHash("IsHolster");
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

    void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_CharacterController = GetComponent<CharacterController>();

        var cameraManager = CameraManager.Instance;
        if (cameraManager != null)
            cameraManager.OnSwitchCam += OnSwitchCam;

        var inputHandler = GetComponent<InputHandler>();
        if (inputHandler != null)
            inputHandler.OnLook += OnLook;
    }

    void Start()
    {
    }


    void Update()
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

        m_CurCamMode = mode;
    }

    void OnLook(Vector2 lookDelta)
    {
        if (m_CurCamMode == CameraMode.FirstPerson)
        {
            FPSModel.transform.Rotate(Vector3.right, -lookDelta.y);
        }

    }

    public void SetHolster(bool isHolster)
    {
        m_Animator.SetBool(m_IsHolsterHash, isHolster);
    }

}
