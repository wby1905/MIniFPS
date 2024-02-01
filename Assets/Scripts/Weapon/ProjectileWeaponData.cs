using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileWeaponData", menuName = "MIniFPS/ActorData/ProjectileWeaponData")]
public class ProjectileWeaponData : WeaponData
{
    public string Ejection;
    public ProjectileBehaviour ProjectilePrefab;
    public ActorBehaviour CasingPrefab;
}