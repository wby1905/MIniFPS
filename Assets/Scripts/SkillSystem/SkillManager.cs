using System.Collections;
using UnityEngine;

// 管理角色拥有的技能及技能状态
public class SkillManager : ActorController
{
    public SkillData[] skills;

    public override void Init(ActorBehaviour ab)
    {
        base.Init(ab);
        PlayerBehaviour pb = ab as PlayerBehaviour;
        if (pb != null)
        {
            skills = pb.skills;
            return;
        }

        EnemyBehaviour eb = ab as EnemyBehaviour;
        if (eb != null)
        {
            skills = eb.skills;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (skills == null) return;
        foreach (var skill in skills)
        {
            InitSkill(skill);
        }
    }

    private void InitSkill(SkillData data)
    {
        data.caster = this.actorBehaviour;
        data.skillState = SkillState.Idle;
    }

    public SkillData PrepareSkill(int index)
    {
        if (skills == null || index < 0 || index >= skills.Length) return null;
        var skill = skills[index];

        if (skill != null && skill.skillState == SkillState.Idle)
        {
            skill.skillState = SkillState.Casting;
            return skill;
        }
        return null;
    }

    public void ResetSkill(int index)
    {
        var skill = skills[index];
        if (skill != null)
        {
            skill.skillState = SkillState.Idle;
        }
    }

    public void GenerateSkill(SkillData data)
    {
        if (data == null || data.skillPrefab == null) return;
        ActorBehaviour skillAb = WorldManager.Instantiate(data.skillPrefab, data.caster.transform, true);
        Object.Destroy(skillAb.gameObject, data.duration + 0.01f);

        SkillDeployer deployer = skillAb.GetController<SkillDeployer>();
        if (deployer != null)
        {
            deployer.SkillData = data;
            if (data.castFx)
            {
                AudioManager.Instance.PlayOneShot(data.castFx, 0.5f, 1f, data.castOrigin.position);
            }
            deployer.DeploySkill();
            data.skillState = SkillState.Cooldown;
            actorBehaviour.StartCoroutine(CoolDown(data));
        }

    }

    private IEnumerator CoolDown(SkillData data)
    {
        data.cdTimer = data.cooldown;
        while (data.cdTimer > 0)
        {
            yield return new WaitForSeconds(0.5f);
            data.cdTimer -= 0.5f;
        }
        data.skillState = SkillState.Idle;
    }
}