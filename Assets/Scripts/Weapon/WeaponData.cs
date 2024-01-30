using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ActorData/WeaponData")]
public class WeaponData : ActorData
{
    public MuzzleData muzzleData;

    public string LeftHandTarget, RightHandTarget;
    public string LeftHandPole, RightHandPole;

    public bool IsAutomatic = false;
    public float FireCoolDown = 0.25f;

    public int CurAmmo = 30;
    public int MaxAmmo = 30;
    public int TotalAmmo = 90;

    public AudioClip[] FireSounds;
    public AudioClip FireEmptySound;

    public AudioClip ReloadSound;
}

