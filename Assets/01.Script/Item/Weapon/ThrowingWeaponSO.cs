using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Items.Weapons
{
    public enum EThrowingType
    {
        Straight,
        Parabola
    }
    public class ThrowingWeaponSO : WeaponSO
    {
        [Header("Parameter Setting")]
        public ProjectilePoolType weapon = ProjectilePoolType.Grenade;
        public float throwingSpeed = 50f;
        public EThrowingType throwingType;
    }
}
