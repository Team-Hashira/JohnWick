using System;
using Crogen.AttributeExtension;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.SkillSystem
{
    public abstract class Skill : MonoBehaviour
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
        
        public virtual void UseSkill()
        {
            currentCoolTime = 0;
            IsReadyCoolTime = false;
        }
    }
}