using UnityEngine;

namespace Hashira.Weapons
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
    public class WeaponSO : ScriptableObject
    {
        public string weaponName;
        public Sprite weaponSprite;
    }
}
