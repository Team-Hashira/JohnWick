using Hashira.Core.StatSystem;
using System.Collections.Generic;
using UnityEngine;
using Doryu.CustomAttributes;

namespace Hashira.Items.Weapons
{
    public abstract class WeaponSO : ItemSO
    {
        [field: Header("==========Weapon setting==========")]
        [field: SerializeField] public Vector3 GrapOffset { get; internal set; }
        [field: SerializeField] public Vector3 RightHandOffset { get; internal set; }
        [field: SerializeField] public Vector3 LeftHandOffset { get; internal set; }
        [field: SerializeField] public float GrapRotate { get; internal set; }
        [Header("Stat")]
        public List<StatElement> overrideStatElementList = new List<StatElement>();
        [VisibleInspectorSO] public StatBaseSO baseStat;

        private void OnValidate()
        {
            for (int i = 0; i < overrideStatElementList.Count; i++)
            {
                overrideStatElementList[i].Name = overrideStatElementList[i].elementSO.displayName;
            }
        }
    }
}
