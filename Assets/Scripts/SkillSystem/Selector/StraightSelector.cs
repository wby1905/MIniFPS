using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StraightSelector : ISelector
{

    public Transform[] SelectTarget(SkillData data, Transform origin)
    {
        HashSet<Transform> targets = new HashSet<Transform>();
        List<Vector3> targetPositions = new List<Vector3>();

        Vector3 originPos = origin.position;
        Quaternion rot = Quaternion.Euler(data.castAngle);
        Vector3 direction = rot * origin.forward;

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
                if (hit.collider.gameObject == origin.gameObject)
                {
                    continue;
                }
                targets.Add(hit.collider.transform);
                Debug.Log(targets.Last().name);
                targetPositions.Add(hit.point);
                if (data.skillType == SkillType.Single)
                {
                    break;
                }
            }
        }

        data.targetPositions = targetPositions.ToArray();
        return targets.ToArray();
    }

}