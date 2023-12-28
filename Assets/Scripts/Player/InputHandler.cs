using UnityEngine;
using Cinemachine;

public class InputHandler : MonoBehaviour
{

    public float GroundSpeed = 5f;

    public float LookSpeed = 100f;
    public float LookPitchLimit = 60f;

    public float JumpForce = 5f;


    public enum CameraMode { FirstPerson, ThirdPerson };
    private CameraMode m_CameraMode = CameraMode.ThirdPerson;
    public CinemachineVirtualCamera FirstPerson;
    public CinemachineVirtualCamera ThirdPerson;
    private CinemachinePOV m_ThirdPov;
    private Camera m_MainCamera;

    private PlayerControl m_PlayerControl;
    private CharacterController m_CharacterController;

    private Vector3 m_Velocity;

    void Awake()
    {
        m_PlayerControl = new PlayerControl();
        m_CharacterController = GetComponent<CharacterController>();

        m_MainCamera = Camera.main;
        m_ThirdPov = ThirdPerson.GetCinemachineComponent<CinemachinePOV>();

        m_PlayerControl.gameplay.SwitchCam.performed += ctx => SwitchCam();
        m_PlayerControl.gameplay.Jump.performed += ctx => Jump();
    }

    void OnEnable()
    {
        m_PlayerControl.Enable();
    }

    void OnDisable()
    {
        m_PlayerControl.Disable();
    }

    void Start()
    {
        // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleGravity();
        HandleLook();
        HandleMove();
        m_CharacterController.Move(m_Velocity * Time.deltaTime);
    }


    void HandleMove()
    {
        Vector2 movement = m_PlayerControl.gameplay.Move.ReadValue<Vector2>();
        Vector3 moveDir = m_MainCamera.transform.forward * movement.y + m_MainCamera.transform.right * movement.x;
        moveDir.y = 0;
        moveDir.Normalize();
        m_Velocity = moveDir * GroundSpeed + m_Velocity.y * Vector3.up;
    }

    void HandleGravity()
    {
        if (m_CharacterController.isGrounded && m_Velocity.y < 0)
        {
            m_Velocity.y = -0.05f;
        }
        else
        {
            m_Velocity.y -= 9.81f * Time.deltaTime;
        }
    }

    void HandleLook()
    {
        Vector2 look = m_PlayerControl.gameplay.Look.ReadValue<Vector2>().normalized;
        if (look.sqrMagnitude < 0.01f)
            return;

        switch (m_CameraMode)
        {
            case CameraMode.FirstPerson:
                if (FirstPerson == null) return;
                transform.Rotate(0, look.x * LookSpeed * Time.deltaTime, 0);
                var curPitch = FirstPerson.transform.eulerAngles.x;
                if (curPitch > 180f) curPitch -= 360f;
                if (curPitch < -180f) curPitch += 360f;
                var deltaPitch = -look.y * LookSpeed * Time.deltaTime;
                if (curPitch + deltaPitch < -LookPitchLimit || curPitch + deltaPitch > LookPitchLimit)
                    return;
                FirstPerson.transform.Rotate(-look.y * LookSpeed * Time.deltaTime, 0, 0);
                break;
            case CameraMode.ThirdPerson:
                if (m_ThirdPov == null) return;
                m_ThirdPov.m_HorizontalAxis.Value += look.x * LookSpeed * Time.deltaTime;
                m_ThirdPov.m_VerticalAxis.Value += look.y * LookSpeed * Time.deltaTime;
                break;
        }

    }

    void Jump()
    {
        if (!m_CharacterController.isGrounded)
            return;
        m_Velocity.y = JumpForce;
    }

    void SwitchCam()
    {
        if (m_CameraMode == CameraMode.ThirdPerson)
        {
            m_CameraMode = CameraMode.FirstPerson;
            FirstPerson.Priority = 10;
            ThirdPerson.Priority = 0;
        }
        else
        {
            m_CameraMode = CameraMode.ThirdPerson;
            FirstPerson.Priority = 0;
            ThirdPerson.Priority = 10;
        }
    }
}
