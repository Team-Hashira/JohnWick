using System;
using Crogen.AttributeExtension;
using UnityEngine;

namespace Hashira.PerkSystem
{
    public abstract class Perk : MonoBehaviour
    {
        //Passive
        
        public bool isPassive;
        //Cool Time
        public bool useCoolTime;
        [HideInInspectorByCondition(nameof(useCoolTime))] public float coolTime = 1f;
        [HideInInspector] public float currentCoolTime = 0;
        [HideInInspector] public bool IsReadyCoolTime;
        public event Action<float> CoolTimePercentEvent;

        //Conditional Event
        public bool useConditionalEvent;
        public Action<object> ConditionalEvent;
        
        //Others
        public PerkUIDataSO perkUIDataSo;
        
        public virtual void UsePerk()
        {
            currentCoolTime = 0;
            IsReadyCoolTime = false;
        }
    }
}