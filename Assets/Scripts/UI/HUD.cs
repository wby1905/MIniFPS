using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HUD : ActorController
{
    private TMP_Text m_CurAmmoText;
    private TMP_Text m_TotalAmmoText;
    private TMP_Text m_WeaponNameText;

    private Image m_WeaponIcon;
    private Image m_MagazineIcon;
    private Image m_CasingIcon;

    private WeaponUIPreset[] m_WeaponUIPresets;
    private Dictionary<WeaponType, WeaponUIPreset> m_WeaponUIDic = new Dictionary<WeaponType, WeaponUIPreset>();
    private PlayerController m_Player;

    public override void Init(ActorBehaviour ab)
    {
        if (!(ab is PlayerBehaviour)) return;
        base.Init(ab);
        PlayerBehaviour pb = ab as PlayerBehaviour;
        m_CurAmmoText = pb.CurAmmoText;
        m_TotalAmmoText = pb.TotalAmmoText;
        m_WeaponNameText = pb.WeaponNameText;
        m_WeaponIcon = pb.WeaponIcon;
        m_MagazineIcon = pb.MagazineIcon;
        m_CasingIcon = pb.CasingIcon;
        m_WeaponUIPresets = pb.WeaponUIPresets;
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (var item in m_WeaponUIPresets)
        {
            if (!m_WeaponUIDic.ContainsKey(item.WeaponType))
                m_WeaponUIDic.Add(item.WeaponType, item);
        }
        m_Player = GetController<PlayerController>();
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
            m_MagazineIcon.sprite = preset.MagazineIcon;
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (m_Player != null)
        {
            m_Player.OnWeaponSwitched -= OnWeaponSwitched;
        }
    }

}