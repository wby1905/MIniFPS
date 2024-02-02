using UnityEngine;
using DitzelGames.FastIK;

public class IKHandler : MonoBehaviour
{
    public FastIKFabric IKFabric_Left, IKFabric_Right;
    public Transform LeftHandTarget
    {
        set
        {
            if (value == null)
                IKFabric_Left.enabled = false;
            else
                IKFabric_Left.enabled = true;
            if (IKFabric_Left != null)
                IKFabric_Left.Target = value;
        }
    }
    public Transform LeftHandPole
    {
        set
        {
            if (IKFabric_Left != null)
                IKFabric_Left.Pole = value;
        }
    }

    public Transform RightHandTarget
    {
        set
        {
            if (value == null)
                IKFabric_Right.enabled = false;
            else
                IKFabric_Right.enabled = true;
            if (IKFabric_Right != null)
                IKFabric_Right.Target = value;
        }
    }

    public Transform RightHandPole
    {
        set
        {
            if (IKFabric_Right != null)
                IKFabric_Right.Pole = value;
        }
    }


}