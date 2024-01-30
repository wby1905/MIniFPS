using System.Collections.Generic;
using UnityEngine;

public enum DecalType
{
    BulletHole,
}


public class DecalManager : Singleton<DecalManager>
{
    private ObjectPool<DecalActor> m_DecalPool;

    [System.Serializable]
    class DecalItem
    {
        public DecalType decalType;
        public Material material;
    }

    [SerializeField]
    private DecalItem[] m_DecalItems;
    private Dictionary<DecalType, DecalItem> m_DecalItemDict = new Dictionary<DecalType, DecalItem>();

    [SerializeField]
    private DecalBehaviour m_DecalPrefab;

    protected override void Awake()
    {
        base.Awake();
        if (m_DecalPrefab != null)
            m_DecalPool = ObjectPoolManager.Instance.CreateOrGetPool<DecalActor>(m_DecalPrefab, 10, transform);

        foreach (var item in m_DecalItems)
        {
            if (!m_DecalItemDict.ContainsKey(item.decalType))
                m_DecalItemDict.Add(item.decalType, item);
        }
    }

    public void SpawnDecal(DecalType decalType, Vector3 position, Quaternion rotation)
    {
        if (!m_DecalItemDict.ContainsKey(decalType)) return;
        if (m_DecalPool != null)
        {
            DecalActor decal = m_DecalPool.Get();
            if (decal != null)
            {
                decal.DecalPool = m_DecalPool;
                decal.Spawn(m_DecalItemDict[decalType].material, position, rotation);
            }
        }
    }
}