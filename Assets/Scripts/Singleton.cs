using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<T>();
            }
            if (m_Instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                m_Instance = go.AddComponent<T>();
            }
            return m_Instance;
        }
    }
    private static T m_Instance;

    protected virtual void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (m_Instance == this)
        {
            m_Instance = null;
        }
    }

}