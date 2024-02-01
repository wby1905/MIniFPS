using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : ActorBehaviour
{

    [Header("Animator")]
    public float LerpSpeed = 5f;
    public float LookError = 5f;
    public float AimSpeed = 5f;
    public GameObject FPSModel;
    public GameObject TPSModel;

    [Header("Controller")]
    public PlayerAnimationEventHandler FPSAnimationEventHandler;
    public PlayerAnimationEventHandler TPSAnimationEventHandler;
    public IKHandler FPSIKHandler, TPSIKHandler;

    public float GroundSpeed = 5f;
    public float RunSpeed = 10f;
    public float LookSpeed = 200f;
    public float LookPitchLimit = 60f;
    public float JumpSpeed = 5f;

    public Vector3 FPSAimOffset = new Vector3(-10f, 0, 0);
    public Vector3 FPSAimOffsetTop = new Vector3(0, 0.08f, 0);
    public Vector3 FPSAimOffsetBottom = new Vector3(0, -0.1f, 0);

    public LayerMask HitLayerMask;
    public float HitDistance = 1000f;

    public float SwitchCoolDown = 0.5f;

    public SkillData[] skills;


    [Header("HUD")]
    public TMP_Text CurAmmoText;
    public TMP_Text TotalAmmoText;
    public TMP_Text WeaponNameText;
    public Image WeaponIcon;
    public Image MagazineIcon;
    public Image CasingIcon;

    [Header("Inventory")]
    public WeaponUIPreset[] WeaponUIPresets;
    public WeaponMap WeaponTable;
    public List<WeaponType> InitialWeapons;
    public int StartWeaponIndex = 0;
    public Transform FPS_WeaponSocket, TPS_WeaponSocket;

}