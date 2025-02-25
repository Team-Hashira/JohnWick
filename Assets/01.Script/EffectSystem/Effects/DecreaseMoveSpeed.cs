using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class DecreaseMoveSpeed : Effect, ICoolTimeEffect
    {
        private StatElement _speedStatElement;

        public float Duration { get; set; } = 5;
        public float Time { get; set; }

        public override int MaxActiveCount => 4;

        public override void Enable()
        {
            base.Enable();
            _speedStatElement = entityStat.StatDictionary["Speed"];
            _speedStatElement.AddModify("DecreaseMoveSpeed", -30.0f, EModifyMode.Percent);
            //TODO 여기에 이펙트
        }

        public override void Disable()
        {
            base.Disable();
            _speedStatElement.RemoveModify("DecreaseMoveSpeed", EModifyMode.Percent);
            //TODO 여기에 이펙트
        }

        public void OnTimeOut()
        {
        }
    }
}
