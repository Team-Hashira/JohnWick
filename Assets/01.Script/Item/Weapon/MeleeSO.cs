using UnityEngine;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "KnifeSO", menuName = "SO/Weapon/Knife")]
    public class MeleeSO : WeaponSO
    {
        [Header("Gun setting")]
        public Vector2 _attackSize;
    }
}
