﻿using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{

    public float GroundSpeed = 5f;

    public float LookSpeed = 200f;
    public float LookPitchLimit = 60f;

    public float JumpSpeed = 5f;

    public UnityAction<Vector2> OnLook;
    public UnityAction OnSwitchPrev;
    public UnityAction OnSwitchNext;

    private PlayerControl m_PlayerControl;
    private CharacterController m_CharacterController;
    private CameraManager m_CameraManager;
    private CinemachineVirtualCamera m_CurCam;

    private Vector3 m_Velocity;

    void Awake()
    {
        m_PlayerControl = new PlayerControl();
        m_CharacterController = GetComponent<CharacterController>();
        m_CameraManager = CameraManager.Instance;

        m_PlayerControl.gameplay.SwitchCam.performed += ctx => SwitchCam();
        m_PlayerControl.gameplay.Jump.performed += ctx => Jump();
        m_PlayerControl.gameplay.Scroll.performed += ctx => Scroll();
        m_PlayerControl.gameplay.SwitchPrev.performed += ctx => SwitchPrev();
        m_PlayerControl.gameplay.SwitchNext.performed += ctx => SwitchNext();
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

        m_CurCam = m_CameraManager.CurrentCamera;
    }

    void Update()
    {
        if (m_CharacterController == null || m_CameraManager == null)
            return;

        HandleGravity();
        HandleLook();
        HandleMove();
        m_CharacterController.Move(m_Velocity * Time.deltaTime);
    }


    void HandleMove()
    {
        Vector2 movement = m_PlayerControl.gameplay.Move.ReadValue<Vector2>();
        Vector3 moveDir;
        if (m_CurCam != null)
            moveDir = m_CurCam.transform.forward * movement.y + m_CurCam.transform.right * movement.x;
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
            m_Velocity.y -= 9.81f * Time.deltaTime;
        }
    }

    void HandleLook()
    {
        Vector2 look = m_PlayerControl.gameplay.Look.ReadValue<Vector2>().normalized;
        if (look.sqrMagnitude < 0.01f || m_CurCam == null)
            return;
        var camMode = m_CameraManager.CurrentCameraMode;

        look *= LookSpeed * Time.deltaTime;

        switch (camMode)
        {
            case CameraMode.FirstPerson:
                transform.Rotate(0, look.x, 0);
                var curPitch = m_CurCam.transform.eulerAngles.x;
                if (curPitch > 180f) curPitch -= 360f;
                if (curPitch < -180f) curPitch += 360f;
                var deltaPitch = -look.y;
                if (curPitch + deltaPitch < -LookPitchLimit || curPitch + deltaPitch > LookPitchLimit)
                    return;
                m_CurCam.transform.Rotate(-look.y, 0, 0);
                break;

            case CameraMode.ThirdPerson:
                var pov = m_CurCam.GetCinemachineComponent<CinemachinePOV>();
                if (pov == null) return;
                pov.m_HorizontalAxis.Value += look.x;
                pov.m_VerticalAxis.Value += -look.y;
                break;
        }

        if (OnLook != null)
            OnLook.Invoke(look);
    }

    void Jump()
    {
        if (!m_CharacterController.isGrounded)
            return;
        m_Velocity.y = JumpSpeed;
    }

    void SwitchCam()
    {
        m_CameraManager.SwitchCam();
        m_CurCam = m_CameraManager.CurrentCamera;
    }

    void Scroll()
    {
        if (OnSwitchPrev == null || OnSwitchNext == null)
            return;
        var scroll = m_PlayerControl.gameplay.Scroll.ReadValue<float>();
        if (scroll > 0)
            SwitchNext();
        else if (scroll < 0)
            SwitchPrev();
    }

    void SwitchPrev()
    {
        if (OnSwitchPrev != null)
            OnSwitchPrev.Invoke();
    }

    void SwitchNext()
    {
        if (OnSwitchNext != null)
            OnSwitchNext.Invoke();
    }
}
