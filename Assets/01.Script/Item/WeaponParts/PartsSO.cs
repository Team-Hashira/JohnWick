using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum EWeaponPartsType
{
    Muzzle,             //총구
    Scope,              //조준경
    Grip,               //손잡이 
    Magazine,           //탄창
    CartridgeBelt,      //탄띠
    Stock,              //개머리판
    Side,               //사이드
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

        protected override void OnEnable()
        {
            base.OnEnable();
            StatDictionary = new StatDictionary(_statList);
        }
    }
}
