using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public WeaponMap WeaponTable;

    public List<WeaponType> InitialWeapons;
    public int StartWeaponIndex = 0;
    public bool IsEquipped => m_CurIdx >= 0 && m_CurIdx < m_Weapons.Count && m_Weapons[m_CurIdx] != null;
    public Weapon CurrentWeapon
    {
        get
        {
            if (m_CurIdx < 0 || m_CurIdx >= m_Weapons.Count)
                return null;
            return m_Weapons[m_CurIdx];
        }
    }
    protected int m_CurIdx = -1;
    public int CurrentWeaponIndex
    {
        get
        {
            return m_CurIdx;
        }
    }

    public Transform WeaponSocket;

    protected List<Weapon> m_Weapons = new List<Weapon>();


    protected virtual void Awake()
    {
        foreach (var weaponType in InitialWeapons)
        {
            var weapon = Instantiate(WeaponTable.WeaponDic[weaponType], transform).GetComponent<Weapon>();
            weapon.Type = weaponType;
            weapon.Init();
            m_Weapons.Add(weapon);
        }

    }

    protected virtual void Start()
    {
        EquipWeapon(StartWeaponIndex);
    }

    public virtual Weapon EquipWeapon(int index)
    {
        // roll over
        if (index == -1) index = m_Weapons.Count - 1;
        if (index == m_Weapons.Count) index = 0;

        if (index < 0 || index >= m_Weapons.Count)
            return null;

        if (m_Weapons[index] == null)
            return null;

        if (m_CurIdx >= 0 && m_Weapons[m_CurIdx] != null)
        {
            m_Weapons[m_CurIdx].OnUnequip();

        }

        m_CurIdx = index;
        m_Weapons[index].transform.SetParent(WeaponSocket, false);
        m_Weapons[index].OnEquip();

        return m_Weapons[index];
    }

    public virtual void UnequipWeapon()
    {
        if (m_CurIdx >= 0 && m_CurIdx < m_Weapons.Count && m_Weapons[m_CurIdx] != null)
        {
            m_Weapons[m_CurIdx].OnUnequip();
            m_CurIdx = -1;
        }
    }

}