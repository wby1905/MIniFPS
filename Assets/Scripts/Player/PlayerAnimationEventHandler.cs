using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public UnityAction Holster;
    public UnityAction ReloadComplete;

    /*
    * Animation Events
    */
    void OnAnimationEndedHolster()
    {
        if (Holster != null)
            Holster.Invoke();
    }

    void OnReloadComplete()
    {
        if (ReloadComplete != null)
            ReloadComplete.Invoke();
    }

}