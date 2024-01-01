using UnityEngine;

public interface IPoolable
{
    void OnInit();
    void OnRecycle();
}
