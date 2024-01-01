using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    public UnityAction OnEjectCasing;


    /*
    * Animation Events
    */
    void OnEject()
    {
        if (OnEjectCasing != null)
            OnEjectCasing.Invoke();
    }
}