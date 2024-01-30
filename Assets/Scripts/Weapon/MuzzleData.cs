using UnityEngine;

[CreateAssetMenu(fileName = "MuzzleData", menuName = "ActorData/MuzzleData")]
public class MuzzleData : ActorData
{
    public string SocketName;
    public GameObject PrefabFlashParticles;
    public int FlashParticlesCount = 5;
    public GameObject PrefabFlashLight;
    public float FlashLightDuration;
    public Vector3 FlashLightOffset;
}
