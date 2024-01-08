using UnityEngine;
using UnityEngine.Events;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    public UnityAction EjectCasing;
    public UnityAction ReloadComplete;


    /*
    * Animation Events
    */
    void OnEject()
    {
        if (EjectCasing != null)
            EjectCasing.Invoke();
    }

    void OnReloadComplete()
    {
        if (ReloadComplete != null)
            ReloadComplete.Invoke();
    }
}