using System;
using UnityEngine;

[Flags]
public enum EWeaponPartsType
{
    Muzzle = 1,             //�ѱ�
    Scope = 2,              //������
    Grip = 4,               //������
    Magazine = 8,           //źâ
    CartridgeBelt = 16,     //ź��
    Stock = 32,             //���Ӹ���
}

namespace Hashira.Weapons
{
    [CreateAssetMenu(fileName = "WeaponPartsSO", menuName = "SO/Weapon/Parts")]
    public class WeaponPartsSO : ItemSO
    {
        [Header("Weapon parts setting")]
        public EWeaponPartsType weaponPartsType;
    }
}
