using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StraightSelector : ISelector
{

    public Transform[] SelectTarget(SkillData data, Transform origin)
    {
        List<Transform> targets = new List<Transform>();
        Vector3 originPos = origin.position;
        Quaternion rot = Quaternion.Euler(data.castAngle);
        Vector3 direction = rot * origin.forward;
        RaycastHit[] hits = Physics.RaycastAll(originPos, direction, data.castDistance, data.affectLayers);
        foreach (RaycastHit hit in hits)
        {
            if (data.affectTags.Any(tag => hit.collider.tag == tag))
            {
                targets.Add(hit.collider.transform);
                if (data.skillType == SkillType.Single)
                {
                    break;
                }
            }
        }
        return targets.ToArray();
    }

}