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
    protected IImpactor[] m_Impactors;

    protected Transform origin;

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

        if (m_SkillData.castOriginName != null) origin = m_SkillData.caster.FindChild(m_SkillData.castOriginName);
        if (origin == null)
        {
            origin = transform;
        }
        m_SkillData.castOrigin = origin;
        transform.parent = origin;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(m_SkillData.castAngle);
    }

    public void CalculateTargets()
    {
        SkillData.targets = m_Selector.SelectTarget(m_SkillData, origin);
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