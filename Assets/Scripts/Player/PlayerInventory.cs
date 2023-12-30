using UnityEngine;

public class PlayerInventory : Inventory
{

    public int MaxWeaponCount = 2;

    public Transform FPS_WeaponSocket;
    public Transform TPS_WeaponSocket;


    protected override void Awake()
    {
        base.Awake();
        MaxWeaponCount = Mathf.Max(MaxWeaponCount, InitialWeapons.Count);

        var cameraManager = CameraManager.Instance;
        if (cameraManager != null)
            cameraManager.OnSwitchCam += OnSwitchCam;

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