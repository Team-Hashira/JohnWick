using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

namespace Hashira.Items.WeaponPartsSystem
{
    [CreateAssetMenu(fileName = "WeaponPartsSO", menuName = "SO/Weapon/Parts")]
    public class WeaponPartsSO : ItemSO, IStatable
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

        private void OnEnable()
        {
            string className = name;
            try
            {
                Type type = Type.GetType("Hashira.Items.WeaponPartsSystem." + className);
                WeaponParts findedWeaponParts = Activator.CreateInstance(type) as WeaponParts;
                findedWeaponParts.Init(this);
                _itemClass = findedWeaponParts;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{className} not found.\n" + 
                                $"Error : {ex.ToString()}");
            }
            StatDictionary = new StatDictionary(_statList);
        }
    }
}
