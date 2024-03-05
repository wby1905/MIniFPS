using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StraightSelector : ISelector
{

    public Transform[] SelectTarget(SkillData data, Transform origin)
    {
        List<Transform> targets = new List<Transform>();
        Vector3 originPos = origin.position;
        //TODO 这里的旋转有一些问题
        // Quaternion rot = Quaternion.Euler(data.castAngle);
        Vector3 direction = origin.forward;

#if UNITY_EDITOR
        Debug.DrawRay(originPos, direction * data.castDistance, Color.red, 1);
#endif

        RaycastHit[] hits = Physics.RaycastAll(originPos, direction, data.castDistance, data.affectLayers);
        foreach (RaycastHit hit in hits)
        {
            if (
                data.affectTags == null ||
                data.affectTags.Length == 0 ||
                data.affectTags.Any(tag => hit.collider.tag == tag)
                )
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