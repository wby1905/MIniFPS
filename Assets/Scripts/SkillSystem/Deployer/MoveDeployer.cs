using UnityEngine;

public class MoveDeployer : SkillDeployer
{
    public override void DeploySkill()
    {
        CalculateTargets();
        ApplyEffects();
    }
}