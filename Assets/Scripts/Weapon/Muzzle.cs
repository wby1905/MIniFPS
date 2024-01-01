using UnityEngine;

public class Muzzle : MonoBehaviour
{

    [SerializeField]
    private Transform m_Socket;

    [SerializeField]
    private GameObject m_PrefabFlashParticles;

    [SerializeField]
    private int m_FlashParticlesCount = 5;

    [SerializeField]
    private GameObject m_PrefabFlashLight;

    [SerializeField]
    private float m_FlashLightDuration;

    [SerializeField]
    private Vector3 m_FlashLightOffset;


    private ParticleSystem m_Particles;
    private Light m_FlashLight;
    private float m_FlashLightTimer = 0f;

    public Transform Socket => m_Socket;

    void Awake()
    {
        if (m_PrefabFlashParticles != null)
        {
            m_Particles = Instantiate(m_PrefabFlashParticles, m_Socket).GetComponent<ParticleSystem>();
            m_Particles.transform.localPosition = default;
        }

        if (m_PrefabFlashLight != null)
        {
            m_FlashLight = Instantiate(m_PrefabFlashLight, m_Socket).GetComponent<Light>();
            m_FlashLight.transform.localPosition = m_FlashLightOffset;
            m_FlashLight.enabled = false;
        }
    }

    void Update()
    {
        if (m_FlashLightTimer > 0f)
        {
            m_FlashLightTimer -= Time.deltaTime;
            if (m_FlashLightTimer <= 0f)
            {
                m_FlashLight.enabled = false;
            }
        }
    }

    public void Fire()
    {
        if (m_Particles != null)
        {
            m_Particles.Emit(m_FlashParticlesCount);
        }

        if (m_FlashLight != null)
        {
            m_FlashLight.enabled = true;
            m_FlashLightTimer = m_FlashLightDuration;
        }
    }

}