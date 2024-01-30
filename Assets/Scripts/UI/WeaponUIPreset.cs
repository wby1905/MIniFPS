using UnityEngine;

[CreateAssetMenu(menuName = "MIniFPS/WeaponUIPreset")]
public class WeaponUIPreset : ScriptableObject
{
    public Sprite WeaponIcon;
    public Sprite MagazineIcon;
    public Sprite CasingIcon;
    public string WeaponName;
    public WeaponType WeaponType;

}