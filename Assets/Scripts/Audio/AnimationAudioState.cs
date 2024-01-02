using UnityEngine;

public class AnimationAudioState : StateMachineBehaviour
{
    public AudioClip[] AudioClips;

    public float Volume = 1f;
    public bool IsVolumeParametrized = false;
    public string VolumeParam;
    private int m_HashVolumeParam = 0;

    public float Pitch = 1f;
    public float Delay = 0f;
    public bool IsLoop = false;


    private AudioManager m_AudioManager;
    private AudioPoolItem m_AudioPoolItem;



    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_AudioManager == null)
            m_AudioManager = AudioManager.Instance;

        if (IsVolumeParametrized && !string.IsNullOrEmpty(VolumeParam) && m_HashVolumeParam == 0)
            m_HashVolumeParam = Animator.StringToHash(VolumeParam);

        if (AudioClips.Length == 0)
            return;

        if (m_HashVolumeParam != 0)
        {
            Volume = animator.GetFloat(m_HashVolumeParam);
        }

        AudioClip clip = AudioClips[Random.Range(0, AudioClips.Length)];
        m_AudioPoolItem = m_AudioManager.PlayOneShot(clip, Volume, Pitch, animator.transform.position, Delay, IsLoop);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_AudioPoolItem != null)
        {
            m_AudioPoolItem.Stop();
            m_AudioPoolItem = null;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_AudioPoolItem == null)
            return;

        if (m_AudioPoolItem.IsPlaying)
        {
            if (m_HashVolumeParam != 0)
            {
                Volume = animator.GetFloat(m_HashVolumeParam);
                m_AudioPoolItem.AudioSource.volume = Volume;
            }
        }
    }
}