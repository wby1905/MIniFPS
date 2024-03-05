using UnityEngine;


public enum SkillType
{
    Single,
    Aoe
}

public enum SelectorType
{
    Straight,
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
    public Vector3 castAngle;
    public string castOriginName;
    [HideInInspector]
    public Transform castOrigin;
    public string[] affectTags;
    public LayerMask affectLayers;
    [HideInInspector]
    public Transform[] targets;
    public string[] impactTypes;
    public float value;
    public float duration;
    [HideInInspector]
    public ActorBehaviour caster;
    public ActorBehaviour skillIndicator;
    public ActorBehaviour skillPrefab;
    public string[] animationNames;
    public SkillType skillType;
    public SelectorType selectorType;
    public Sprite skillIcon;
    public DestroyType destroyType;
    [HideInInspector]
    public SkillState skillState;
}