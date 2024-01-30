using UnityEngine;

public class PlayerInventory : Inventory
{

    public int MaxWeaponCount = 2;

    public Transform FPS_WeaponSocket;
    public Transform TPS_WeaponSocket;

    private CameraManager m_CameraManager;

    public override void Init(ActorBehaviour ab)
    {
        if (!(ab is PlayerBehaviour)) return;
        base.Init(ab);
        PlayerBehaviour pb = ab as PlayerBehaviour;
        FPS_WeaponSocket = pb.FPS_WeaponSocket;
        TPS_WeaponSocket = pb.TPS_WeaponSocket;
        m_WeaponTable = pb.WeaponTable;
        m_InitialWeapons = pb.InitialWeapons;
        m_StartWeaponIndex = pb.StartWeaponIndex;
    }

    protected override void Awake()
    {
        base.Awake();
        MaxWeaponCount = Mathf.Max(MaxWeaponCount, m_InitialWeapons.Count);

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