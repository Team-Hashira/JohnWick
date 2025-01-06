using System;

namespace Hashira.SkillSystem
{
    public abstract class Skill
    {
        public Skill(bool isPassive, bool useCoolTime, bool useConditionalEvent)
        {
            this.isPassive = isPassive;
            this.useCoolTime = useCoolTime;
            this.useConditionalEvent = useConditionalEvent;
        }

        //Passive
        public bool isPassive;
        
        //Cool Time
        public bool useCoolTime;
        public float coolTime = 1f;
        protected float _currentCoolTime = 0;
        public bool IsReadyCoolTime;
        public event Action<float> CoolTimePercentEvent;
        
        //Conditional Event
        public bool useConditionalEvent;

        public virtual void UseSkill()
        {
            IsReadyCoolTime = false;
        }
    }
}