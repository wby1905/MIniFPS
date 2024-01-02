using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    Casing,
    BulletImpact,
}

public class AudioManager : Singleton<AudioManager>
{
    [System.Serializable]
    public class AudioTypeClip
    {
        public AudioType audioType;
        public AudioClip[] audioClips;
    }

    [SerializeField]
    private AudioTypeClip[] m_AudioTypeClips;
    private Dictionary<AudioType, AudioClip[]> m_AudioClipsDict = new Dictionary<AudioType, AudioClip[]>();

    [SerializeField]
    private AudioPoolItem m_AudioPoolItemPrefab;

    private ObjectPool<AudioPoolItem> m_AudioPool;


    protected override void Awake()
    {
        base.Awake();

        foreach (AudioTypeClip audioTypeClip in m_AudioTypeClips)
        {
            if (!m_AudioClipsDict.ContainsKey(audioTypeClip.audioType))
                m_AudioClipsDict.Add(audioTypeClip.audioType, audioTypeClip.audioClips);
        }

        m_AudioPool = ObjectPoolManager.Instance.CreateOrGetPool<AudioPoolItem>(m_AudioPoolItemPrefab, 10);
    }

    public AudioPoolItem PlayOneShot(AudioClip clip, float volume = 1, float pitch = 1, Vector3 position = default, float delay = 0f, bool isLoop = false)
    {
        if (clip == null || m_AudioPool == null)
            return null;
        AudioPoolItem audioPoolItem = m_AudioPool.Get();
        if (audioPoolItem == null)
            return null;
        audioPoolItem.transform.position = position;
        audioPoolItem.Play(clip, volume, pitch, delay, isLoop);
        return audioPoolItem;
    }

    public AudioPoolItem PlayOneShot(AudioType audioType, float volume = 1, float pitch = 1, Vector3 position = default, float delay = 0f, bool isLoop = false)
    {
        if (!m_AudioClipsDict.ContainsKey(audioType))
            return null;

        AudioClip[] audioClips = m_AudioClipsDict[audioType];
        if (audioClips.Length == 0)
            return null;

        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        return PlayOneShot(clip, volume, pitch, position, delay, isLoop);
    }

}