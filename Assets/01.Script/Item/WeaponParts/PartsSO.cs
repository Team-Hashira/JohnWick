using Hashira.Core.StatSystem;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponPartsType
{
    Muzzle,             //�ѱ�
    Scope,              //������
    Grip,               //������ 
    Magazine,           //źâ
    CartridgeBelt,      //ź��
    Stock,              //���Ӹ���
}

namespace Hashira.Items.PartsSystem
{
    [CreateAssetMenu(fileName = "PartsSO", menuName = "SO/Weapon/Parts")]
    public class PartsSO : ItemSO, IStatable
    {
        [Header("==========Weapon parts setting==========")]
        public EWeaponPartsType partsType;

        [SerializeField] private List<StatElement> _statList = new List<StatElement>();
        public StatDictionary StatDictionary { get; private set; }

        private void OnValidate()
        {
            for (int i = 0; i < _statList.Count; i++)
            {
                _statList[i].Name = _statList[i].elementSO.displayName;
            }
        }
    }
}
