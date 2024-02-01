using System;
using UnityEngine;

// 用来释放技能
public abstract class SkillDeployer : ActorController
{
    protected SkillData m_SkillData;
    public SkillData SkillData
    {
        get { return m_SkillData; }
        set { m_SkillData = value; InitDeployer(); }
    }

    protected ISelector m_Selector;
    private IImpactor[] m_Impactors;

    private void InitDeployer()
    {
        Type selectorType = Type.GetType(m_SkillData.selectorType + "Selector");
        m_Selector = Activator.CreateInstance(selectorType) as ISelector;

        m_Impactors = new IImpactor[m_SkillData.impactTypes.Length];
        for (int i = 0; i < m_SkillData.impactTypes.Length; i++)
        {
            Type impactorType = Type.GetType(m_SkillData.impactTypes[i] + "Impactor");
            m_Impactors[i] = Activator.CreateInstance(impactorType) as IImpactor;
        }
    }

    public void CalculateTargets()
    {
        SkillData.targets = m_Selector.SelectTarget(m_SkillData, transform);
    }

    public void ApplyEffects()
    {
        for (int i = 0; i < m_Impactors.Length; i++)
        {
            m_Impactors[i].Impact(this);
        }
    }

    public abstract void DeploySkill();
}