using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "MeleeSO", menuName = "SO/Weapon/Melee")]
    public class MeleeSO : WeaponSO
    {
        public Vector2 attackRange;
        public EffectPoolType attackEffect;
    }
}
