using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MIniFPS/WeaponMap")]
public class WeaponMap : ScriptableObject
{
    [System.Serializable]
    public struct WeaponTable
    {
        public WeaponType Type;
        public WeaponData WeaponPrefab;
    }

    [SerializeField]
    private WeaponTable[] WeaponPrefabs;

    public Dictionary<WeaponType, WeaponData> WeaponDic
    {
        get
        {
            if (m_WeaponDic == null)
            {
                m_WeaponDic = new Dictionary<WeaponType, WeaponData>();
                foreach (var weapon in WeaponPrefabs)
                {
                    if (m_WeaponDic.ContainsKey(weapon.Type))
                        continue;
                    m_WeaponDic.Add(weapon.Type, weapon.WeaponPrefab);
                }
            }
            return m_WeaponDic;

        }
    }
    private Dictionary<WeaponType, WeaponData> m_WeaponDic;
}