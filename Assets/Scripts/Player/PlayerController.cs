using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }

    private InputHandler m_InputHandler;
    private PlayerInventory m_Inventory;
    private PlayerAnimator m_PlayerAnimator;
    public PlayerAnimationEventHandler FPSAnimationEventHandler, TPSAnimationEventHandler;

    private bool m_IsHolster = false;
    private int m_WeaponIdx = 0;
    public float SwitchCoolDown = 0.5f;
    private float m_SwitchTimer = 0f;



    void Awake()
    {
        m_InputHandler = GetComponent<InputHandler>();
        m_Inventory = GetComponent<PlayerInventory>();
        m_PlayerAnimator = GetComponent<PlayerAnimator>();

        if (m_InputHandler != null)
        {
            m_InputHandler.OnSwitchNext += () => TrySwitchWeapon(m_Inventory.CurrentWeaponIndex + 1);
            m_InputHandler.OnSwitchPrev += () => TrySwitchWeapon(m_Inventory.CurrentWeaponIndex - 1);
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

    }

    void Update()
    {
        if (m_SwitchTimer > 0f)
            m_SwitchTimer -= Time.deltaTime;
    }


    bool IsIdle()
    {
        return CurrentState == PlayerState.Idle;
    }


    void TrySwitchWeapon(int index)
    {
        if (m_Inventory == null || !IsIdle() || m_SwitchTimer > 0f)
            return;

        CurrentState = PlayerState.Switching;
        m_WeaponIdx = index;
        m_IsHolster = true;
        m_PlayerAnimator.SetHolster(true);
        m_SwitchTimer = SwitchCoolDown;
    }

    void OnHolster()
    {
        // Equip finished (gun raise)
        if (!m_IsHolster)
        {
            // Switch weapon
            if (CurrentState != PlayerState.Switching)
                return;
            m_IsHolster = true;
            m_PlayerAnimator.SetHolster(true);
        }
        // Unequip finished (gun lower)
        else
        {
            if (m_Inventory != null)
                m_Inventory.EquipWeapon(m_WeaponIdx);
            CurrentState = PlayerState.Idle;
            m_IsHolster = false;
            m_PlayerAnimator.SetHolster(false);
            m_SwitchTimer = SwitchCoolDown;
        }
    }


}