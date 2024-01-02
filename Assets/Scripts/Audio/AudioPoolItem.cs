using UnityEngine;

public class AudioPoolItem : MonoBehaviour, IPoolable
{
    private AudioSource m_AudioSource;
    private ObjectPool<AudioPoolItem> m_Pool;

    public AudioSource AudioSource => m_AudioSource;
    public bool IsPlaying => m_AudioSource.isPlaying;


    void Update()
    {
        if (!m_AudioSource.isPlaying)
            Recycle();
    }

    public void OnInit()
    {
        if (m_AudioSource == null)
            m_AudioSource = GetComponent<AudioSource>();
    }

    public void OnRecycle()
    {
        m_AudioSource.Stop();
        m_AudioSource.clip = null;
        m_AudioSource.volume = 1f;
        m_AudioSource.pitch = 1f;
        m_AudioSource.loop = false;
        m_AudioSource.playOnAwake = false;
    }

    public void Play(AudioClip clip, float volume, float pitch, float delay, bool isLoop)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.volume = volume;
        m_AudioSource.pitch = pitch;
        m_AudioSource.loop = isLoop;
        m_AudioSource.PlayDelayed(delay);
    }

    public void Stop()
    {
        m_AudioSource.Stop();
        Recycle();
    }

    public void SetPool(ObjectPool<AudioPoolItem> pool)
    {
        m_Pool = pool;
    }

    public void Recycle()
    {
        if (m_Pool != null)
            m_Pool.Recycle(this);
        else
            Destroy(gameObject);
    }

}