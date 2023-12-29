using UnityEngine;

public class AnimationController : MonoBehaviour
{

    private Animator m_Animator;
    private CharacterController m_CharacterController;
    private CameraManager m_CameraManager;
    private InputHandler m_InputHandler;

    private int m_VelocityX = Animator.StringToHash("VelocityX");
    private int m_VelocityZ = Animator.StringToHash("VelocityZ");
    private int m_IsGrounded = Animator.StringToHash("IsGrounded");

    public GameObject FPSModel;
    public GameObject TPSModel;

    void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_CharacterController = GetComponent<CharacterController>();

        m_CameraManager = GetComponent<CameraManager>();
        if (m_CameraManager != null)
            m_CameraManager.OnSwitchCam += OnSwitchCam;

        m_InputHandler = GetComponent<InputHandler>();
        if (m_InputHandler != null)
            m_InputHandler.OnLook += OnLook;
    }


    void Update()
    {
        if (m_CharacterController == null || m_Animator == null)
            return;

        var velocity = m_CharacterController.velocity;
        var VelocityX = Vector3.Dot(velocity, transform.right);
        var VelocityZ = Vector3.Dot(velocity, transform.forward);

        m_Animator.SetFloat(m_VelocityX, VelocityX);
        m_Animator.SetFloat(m_VelocityZ, VelocityZ);
        m_Animator.SetBool(m_IsGrounded, m_CharacterController.isGrounded);

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

    }

    void OnLook(Vector2 lookDelta)
    {
        if (m_CameraManager.CurrentCameraMode == CameraMode.FirstPerson)
        {
            FPSModel.transform.Rotate(Vector3.right, -lookDelta.y);
        }

    }
}
