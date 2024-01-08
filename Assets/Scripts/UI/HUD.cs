using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_CurAmmoText;

    [SerializeField]
    private TMP_Text m_TotalAmmoText;

    [SerializeField]
    private TMP_Text m_WeaponNameText;

    [SerializeField]
    private Image m_WeaponIcon;
    [SerializeField]
    private Image m_CasingIcon;

    [SerializeField]
    private WeaponUIPreset[] m_WeaponUIPresets;
    private Dictionary<WeaponType, WeaponUIPreset> m_WeaponUIDic = new Dictionary<WeaponType, WeaponUIPreset>();

    [SerializeField]
    private PlayerController m_Player;

    void Awake()
    {
        foreach (var item in m_WeaponUIPresets)
        {
            if (!m_WeaponUIDic.ContainsKey(item.WeaponType))
                m_WeaponUIDic.Add(item.WeaponType, item);
        }

        if (m_Player != null)
        {
            m_Player.OnWeaponSwitched += OnWeaponSwitched;
        }
    }

    void OnWeaponSwitched(Weapon weapon)
    {
        if (weapon == null)
            return;
        WeaponType type = weapon.Type;
        if (m_WeaponUIDic.ContainsKey(type))
        {
            WeaponUIPreset preset = m_WeaponUIDic[type];
            m_WeaponIcon.sprite = preset.WeaponIcon;
            m_CasingIcon.sprite = preset.CasingIcon;
            m_WeaponNameText.text = preset.WeaponName;
        }
        weapon.OnAmmoChanged += OnAmmoChanged;
        OnAmmoChanged(weapon.CurAmmo, weapon.TotalAmmo);
    }

    void OnAmmoChanged(int curAmmo, int totalAmmo)
    {
        m_CurAmmoText.text = curAmmo.ToString();
        m_TotalAmmoText.text = totalAmmo.ToString();
    }


}