using UnityEngine;

namespace Hashira.Weapons
{
    [CreateAssetMenu(fileName = "KnifeSO", menuName = "SO/Weapon/Knife")]
    public class KnifeSO : WeaponSO
    {
        [Header("Gun setting")]
        public Vector2 _attackSize;
    }
}
