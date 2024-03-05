using System;
using UnityEngine;

public class DamageImpactor : IImpactor
{
    public void Impact(SkillDeployer deployer)
    {
        var data = deployer.SkillData;
        var targets = data.targets;

        if (targets == null || targets.Length == 0)
        {
            return;
        }

        if (data.skillType == SkillType.Single)
        {
            var target = targets[0];
            //TODO: 伤害系统
            Debug.Log("Single Impact: " + target.name);
        }
        else if (data.skillType == SkillType.Aoe)
        {
            foreach (var target in targets)
            {
                Debug.Log("Aoe Impact: " + target.name);
            }
        }
    }
}