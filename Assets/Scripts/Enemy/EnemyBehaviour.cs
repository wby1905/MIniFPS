using UnityEngine;

public class EnemyBehaviour : ActorBehaviour
{

    public string TargetTag = "Player";

    [Header("Sight")]
    public string EyeBone;
    public float MaxSightDistance = 10f;
    public float FOV = 45f;
    public LayerMask LayerMask;

    [Header("Combat")]
    public string WeaponSocket;
    public float AttackRange = 2f;
    public Vector3[] PatrolPoints;
    public float WaitTime = 3f;
    public SkillData[] skills;
    public float UpdateRate = 0.5f;

}