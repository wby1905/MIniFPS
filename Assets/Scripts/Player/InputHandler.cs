using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public UnityAction<Vector2> OnMove;
    public UnityAction<Vector2> OnLook;
    public UnityAction OnJump;
    public UnityAction OnSwitchPrev;
    public UnityAction OnSwitchNext;
    public UnityAction OnStartFire;
    public UnityAction OnStopFire;
    public UnityAction OnStartAim;
    public UnityAction OnStopAim;
    public UnityAction OnReload;
    public UnityAction OnStartRun;
    public UnityAction OnStopRun;

    private PlayerControl m_PlayerControl;
    private CameraManager m_CameraManager;
    private PlayerController m_PlayerController;

    public Vector2 MoveInput => m_PlayerControl.gameplay.Move.ReadValue<Vector2>();
    public Vector2 LookInput => m_PlayerControl.gameplay.Look.ReadValue<Vector2>();

    void Awake()
    {
        m_PlayerControl = new PlayerControl();
        m_PlayerController = GetComponent<PlayerController>();
        m_CameraManager = CameraManager.Instance;

        /**
        * gameplay inputs
        */
        m_PlayerControl.gameplay.Move.performed += ctx => Move();
        m_PlayerControl.gameplay.Look.performed += ctx => Look();
        m_PlayerControl.gameplay.Jump.performed += ctx => Jump();

        m_PlayerControl.gameplay.SwitchCam.performed += ctx => SwitchCam();

        m_PlayerControl.gameplay.Scroll.performed += ctx => Scroll();
        m_PlayerControl.gameplay.SwitchPrev.performed += ctx => SwitchPrev();
        m_PlayerControl.gameplay.SwitchNext.performed += ctx => SwitchNext();

        m_PlayerControl.gameplay.Fire.started += ctx => StartFire();
        m_PlayerControl.gameplay.Fire.canceled += ctx => StopFire();

        m_PlayerControl.gameplay.Aim.started += ctx => StartAim();
        m_PlayerControl.gameplay.Aim.canceled += ctx => StopAim();
        m_PlayerControl.gameplay.Reload.performed += ctx => Reload();

        m_PlayerControl.gameplay.Run.started += ctx => StartRun();
        m_PlayerControl.gameplay.Run.canceled += ctx => StopRun();

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
        if (m_PlayerControl.gameplay.enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    void Move()
    {
        if (OnMove != null)
            OnMove.Invoke(MoveInput);
    }

    void Look()
    {
        if (OnLook != null)
            OnLook.Invoke(LookInput);
    }

    void Jump()
    {
        if (OnJump != null)
            OnJump.Invoke();
    }

    void SwitchCam()
    {
        // only switch cam if player is idle
        // maybe remove this when fps and tps are updated simultaneously
        if (m_PlayerController.IsIdle && m_CameraManager != null)
            m_CameraManager.SwitchCam();
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

    void StartFire()
    {
        if (OnStartFire != null)
            OnStartFire.Invoke();
    }

    void StopFire()
    {
        if (OnStopFire != null)
            OnStopFire.Invoke();
    }

    void StartAim()
    {
        if (OnStartAim != null)
            OnStartAim.Invoke();
    }

    void StopAim()
    {
        if (OnStopAim != null)
            OnStopAim.Invoke();
    }

    void Reload()
    {
        if (OnReload != null)
            OnReload.Invoke();
    }

    void StartRun()
    {
        if (OnStartRun != null)
            OnStartRun.Invoke();
    }

    void StopRun()
    {
        if (OnStopRun != null)
            OnStopRun.Invoke();
    }
}
