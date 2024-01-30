using UnityEngine;

public class Muzzle : ActorController
{

    private Transform m_Socket;
    private GameObject m_PrefabFlashParticles;
    private int m_FlashParticlesCount = 5;
    private GameObject m_PrefabFlashLight;
    private float m_FlashLightDuration;
    private Vector3 m_FlashLightOffset;


    private ParticleSystem m_Particles;
    private Light m_FlashLight;
    private float m_FlashLightTimer = 0f;

    public Transform Socket => m_Socket;

    public override void ConfigData(ActorData data)
    {
        if (data is WeaponData wd) ConfigData(wd.muzzleData);
        base.ConfigData(data);
        MuzzleData md = data as MuzzleData;
        if (md != null)
        {
            m_Socket = actorBehaviour.FindChild(md.SocketName);
#if UNITY_EDITOR
            if (m_Socket == null)
            {
                Debug.LogError("MuzzleData SocketName is not found");
            }
#endif
            m_PrefabFlashParticles = md.PrefabFlashParticles;
            m_FlashParticlesCount = md.FlashParticlesCount;
            m_PrefabFlashLight = md.PrefabFlashLight;
            m_FlashLightDuration = md.FlashLightDuration;
            m_FlashLightOffset = md.FlashLightOffset;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (m_PrefabFlashParticles != null)
        {
            m_Particles = GameObject.Instantiate(m_PrefabFlashParticles, m_Socket).GetComponent<ParticleSystem>();
            m_Particles.transform.localPosition = default;
        }

        if (m_PrefabFlashLight != null)
        {
            m_FlashLight = GameObject.Instantiate(m_PrefabFlashLight, m_Socket).GetComponent<Light>();
            m_FlashLight.transform.localPosition = m_FlashLightOffset;
            m_FlashLight.enabled = false;
        }
    }

    protected override void Update()
    {
        base.Update();
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