using UnityEngine;


// 用来释放技能
public class SkillSystem : ActorController
{
    private SkillManager m_SkillManager;
    private Animator m_Animator;

    private SkillData m_Skill;
    private int m_SkillCounter;

    protected override void Awake()
    {
        base.Awake();
        m_SkillManager = actorBehaviour.GetController<SkillManager>();
        if (m_SkillManager == null)
        {
            Debug.LogError("SkillSystem: SkillManager not found!");
            return;
        }
        // get the first active animator
        var animators = actorBehaviour.GetComponentsInChildren<Animator>();
        foreach (var anim in animators)
        {
            if (anim.gameObject.activeInHierarchy)
            {
                m_Animator = anim;
                break;
            }
        }

        m_SkillCounter = 0;
    }

    public void DeploySkill()
    {
        m_SkillManager.GenerateSkill(m_Skill);
    }

    public bool StartIndicator(int skillIdx)
    {
        if (m_Skill == null || m_Skill.skillIndicator == null)
        {
            return false;
        }

        m_Skill = m_SkillManager.PrepareSkill(skillIdx);

        //TODO: skill indicator
        return true;
    }

    public void EndIndicator()
    {
        //TODO: skill indicator
    }

    public float ReleaseSkill(int skillIdx)
    {
        m_Skill = m_SkillManager.PrepareSkill(skillIdx);
        if (m_Skill == null) return 0f;
        if (m_Skill.animationNames.Length > 0)
        {
            m_Animator.CrossFade(m_Skill.animationNames[m_SkillCounter], 0.1f);
            m_SkillCounter = (m_SkillCounter + 1) % m_Skill.animationNames.Length;
        }
        DeploySkill();
        return m_Skill.cooldown;
    }

}