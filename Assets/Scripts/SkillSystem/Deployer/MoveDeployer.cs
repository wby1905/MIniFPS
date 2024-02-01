using UnityEngine;

public class MoveDeployer : SkillDeployer
{
    public override void DeploySkill()
    {
        CalculateTargets();
        ApplyEffects();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Debug.Log("MoveDeployer OnDestroy");
    }
}