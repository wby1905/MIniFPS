using UnityEngine;

public class PlayerInventory : Inventory
{

    public int MaxWeaponCount = 2;

    public Transform FPS_WeaponSocket;
    public Transform TPS_WeaponSocket;

    private InputHandler m_InputHandler;
    private CameraManager m_CameraManager;

    protected override void Awake()
    {
        base.Awake();
        MaxWeaponCount = Mathf.Max(MaxWeaponCount, InitialWeapons.Count);
        m_InputHandler = GetComponent<InputHandler>();

        m_CameraManager = CameraManager.Instance;
        if (m_CameraManager != null)
            m_CameraManager.OnSwitchCam += OnSwitchCam;

        if (m_InputHandler != null)
        {
            m_InputHandler.OnSwitchPrev += () => EquipWeapon(m_CurIdx - 1);
            m_InputHandler.OnSwitchNext += () => EquipWeapon(m_CurIdx + 1);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    void OnSwitchCam(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.FirstPerson:
                WeaponSocket = FPS_WeaponSocket;
                break;
            case CameraMode.ThirdPerson:
                WeaponSocket = TPS_WeaponSocket;
                break;
        }

        EquipWeapon(CurrentWeaponIndex);
    }

}