using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public WeaponMap WeaponTable;

    public List<WeaponType> InitialWeapons;
    public int StartWeaponIndex = 0;
    public Weapon CurrentWeapon
    {
        get
        {
            return m_Weapons[m_CurIdx];
        }
    }
    private int m_CurIdx = -1;
    public int CurrentWeaponIndex
    {
        get
        {
            return m_CurIdx;
        }
    }

    public Transform WeaponSocket;

    private List<Weapon> m_Weapons = new List<Weapon>();


    protected virtual void Awake()
    {
        foreach (var weaponType in InitialWeapons)
        {
            var weapon = Instantiate(WeaponTable.WeaponDic[weaponType], transform).GetComponent<Weapon>();
            weapon.gameObject.SetActive(false);
            m_Weapons.Add(weapon);
        }

    }

    protected virtual void Start()
    {
        EquipWeapon(StartWeaponIndex);
    }

    public virtual void EquipWeapon(int index)
    {
        // 没有检测相等的情况，因为父socket可能会变
        if (index < 0 || index >= m_Weapons.Count)
            return;

        if (m_Weapons[index] == null)
            return;

        if (m_CurIdx >= 0 && m_Weapons[m_CurIdx] != null)
            m_Weapons[m_CurIdx].gameObject.SetActive(false);

        m_CurIdx = index;
        m_Weapons[index].transform.SetParent(WeaponSocket, false);
        m_Weapons[index].gameObject.SetActive(true);
    }

}