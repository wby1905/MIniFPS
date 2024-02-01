using UnityEngine;

public interface ISelector
{
    Transform[] SelectTarget(SkillData data, Transform origin);
}