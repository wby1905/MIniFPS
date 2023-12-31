using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform LeftHandTarget, RightHandTarget;
    public Transform LeftHandPole, RightHandPole;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 100f);
    }

}