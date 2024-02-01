using UnityEngine;


public enum SkillType
{
    Target,
    Aoe
}

public enum SelectorType
{
    None,
    Sector,
    Cylinder,
}

public enum DestroyType
{
    None,
    Time,
    Distance,
    Collision
}

public enum SkillState
{
    Idle,
    Casting,
    Cooldown
}


[CreateAssetMenu(menuName = "MIniFPS/Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public string description;
    public float cooldown;
    [HideInInspector]
    public float cdTimer;
    public float castDistance;
    public float castAngle;
    public string[] affectTags;
    [HideInInspector]
    public Transform[] targets;
    public string[] impactTypes;
    public float damage;
    public float duration;
    [HideInInspector]
    public ActorBehaviour caster;
    public ActorBehaviour skillIndicator;
    public ActorBehaviour skillPrefab;
    public string[] animationNames;
    public SkillType skillType;
    public SelectorType selectorType;
    public string skillIconName;
    public DestroyType destroyType;
    public SkillState skillState;
}