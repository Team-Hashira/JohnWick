using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    [CreateAssetMenu(fileName = "MeleeSO", menuName = "SO/Item/Weapon/Melee")]
    public class MeleeSO : WeaponSO
    {
        [field:SerializeField] public Vector2 AttackRangeOffset { get; private set; }
        [field: SerializeField] public Vector2 AttackRangeSize { get; private set; } = Vector2.one;
        [field:SerializeField] public float AttackDuration { get; private set; }
        [field:SerializeField] public float AttackAfterDelay { get; private set; }
        [field:SerializeField] public float RotateMax { get; private set; }
        [field:SerializeField] public float RotateMin { get; private set; }
        [field:SerializeField] public float Stab { get; private set; }
        [field:SerializeField] public EffectPoolType AttackEffect { get; private set; }
    }
}
