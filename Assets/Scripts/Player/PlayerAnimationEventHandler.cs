using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public UnityAction OnHolster;
    public UnityAction OnEject;

    /*
    * Animation Events
    */
    void OnAnimationEndedHolster()
    {
        if (OnHolster != null)
            OnHolster.Invoke();
    }

    void OnEjectCasing()
    {
        if (OnEject != null)
            OnEject.Invoke();
    }
}