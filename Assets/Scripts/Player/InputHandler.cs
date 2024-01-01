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
    public UnityAction OnFire;

    private PlayerControl m_PlayerControl;
    private CameraManager m_CameraManager;

    public Vector2 MoveInput
    {
        get
        {
            return m_PlayerControl.gameplay.Move.ReadValue<Vector2>();
        }
    }

    public Vector2 LookInput
    {
        get
        {
            return m_PlayerControl.gameplay.Look.ReadValue<Vector2>();
        }
    }



    void Awake()
    {
        m_PlayerControl = new PlayerControl();
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
        m_PlayerControl.gameplay.Fire.performed += ctx => Fire();
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

    void Fire()
    {
        if (OnFire != null)
            OnFire.Invoke();
    }
}
