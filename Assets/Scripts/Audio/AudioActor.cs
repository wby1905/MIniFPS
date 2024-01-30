using UnityEngine;

public class AudioActor : ActorController, IPoolable
{
    private AudioSource m_AudioSource;
    private ObjectPool<AudioActor> m_Pool;

    public AudioSource AudioSource => m_AudioSource;
    public bool IsPlaying => m_AudioSource.isPlaying;
    protected override void Update()
    {
        base.Update();
        if (!m_AudioSource.isPlaying)
            Recycle();
    }

    public override void Init(ActorBehaviour poolItem)
    {
        base.Init(poolItem);
        m_AudioSource = GetComponent<AudioSource>();
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

    public void SetPool(ObjectPool<AudioActor> pool)
    {
        m_Pool = pool;
    }

    public void Recycle()
    {
        if (m_Pool != null)
            m_Pool.Recycle(this);
        else
            Destroy(this);
    }
}