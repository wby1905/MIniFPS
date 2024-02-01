using System;
using System.Collections;
using UnityEngine;

// move caster to the first target
public class MoveImpactor : IImpactor
{
    private SkillData m_Data;
    public void Impact(SkillDeployer deployer)
    {
        m_Data = deployer.SkillData;

        if (m_Data.targets.Length > 0)
        {
            Transform target = m_Data.targets[0];
            Vector3 targetPos = target.position;
            deployer.actorBehaviour.StartCoroutine(Move(targetPos));
        }
        else if (m_Data.skillType == SkillType.Aoe)
        {
            Vector3 targetPos = deployer.actorBehaviour.transform.position + deployer.actorBehaviour.transform.forward * m_Data.castDistance;
            deployer.actorBehaviour.StartCoroutine(Move(targetPos));
        }

    }

    IEnumerator Move(Vector3 targetPos)
    {
        float time = 0f;
        ActorBehaviour caster = m_Data.caster;
        var cc = caster.GetComponent<CharacterController>();

        Vector3 curPos = caster.transform.position;
        while (time < m_Data.duration && Vector3.Distance(caster.transform.position, targetPos) > 0.5f)
        {
            time += Time.deltaTime;
            if (cc != null)
            {
                cc.Move((targetPos - curPos) * Time.deltaTime / m_Data.duration);
            }
            else
            {
                caster.transform.position = Vector3.Lerp(curPos, targetPos, time / m_Data.duration);

            }
            yield return null;
        }

    }
}