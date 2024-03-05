using System;
using System.Collections.Generic;
using UnityEngine;


// 扫描释放，适用于在持续时间内频繁释放技能
public class SweepDeployer : SkillDeployer
{
    // 扫描间隔暂时固定
    private readonly float INTERVAL = 0.05f;
    private float m_PulseTimer;
    private float m_Duration;

    private bool m_IsDeploying;
    private HashSet<int> m_Impacted = new HashSet<int>();
    private List<Transform> m_Targets = new List<Transform>();

    public override void DeploySkill()
    {
        m_IsDeploying = true;
        m_Targets.Clear();
        m_Impacted.Clear();
    }

    protected override void Update()
    {
        base.Update();

        if (m_IsDeploying && m_Duration < m_SkillData.duration)
        {
            m_PulseTimer += Time.deltaTime;
            m_Duration += Time.deltaTime;
            if (m_PulseTimer >= INTERVAL)
            {
                m_PulseTimer = 0;
                CalculateTargets();
                if (SkillData.impactOnce)
                {
                    m_Targets.Clear();
                    foreach (var target in SkillData.targets)
                    {
                        var id = target.root.GetHashCode();
                        if (m_Impacted.Contains(id))
                        {
                            continue;
                        }
                        m_Impacted.Add(id);
                        m_Targets.Add(target);
                    }
                }
                ApplyEffects();
            }
        }

    }
}