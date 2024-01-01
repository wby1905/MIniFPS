using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public UnityAction OnHolster;

    /*
    * Animation Events
    */
    void OnAnimationEndedHolster()
    {
        if (OnHolster != null)
            OnHolster.Invoke();
    }

}