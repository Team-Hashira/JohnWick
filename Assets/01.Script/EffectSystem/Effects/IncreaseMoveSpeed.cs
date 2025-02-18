using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class IncreaseMoveSpeed : Effect, ICoolTimeEffect, ILoopEffect
    {
        private StatElement _speedStatElement;

        public float Duration { get; set; } = 5;
        public float Time { get; set; }

        public override void Enable()
        {
            base.Enable();
            _speedStatElement = entityStat.StatDictionary["Speed"];
            _speedStatElement.AddModify("IncreaseMoveSpeed", 10.0f * level, EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }

        public override void Update()
        {
            base.Update();
            if(Input.GetKeyDown(KeyCode.H))
            {
            }
        }

        public override void Disable()
        {
            base.Disable();
            _speedStatElement.RemoveModify("IncreaseMoveSpeed", EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }

        public void OnTimeOut()
        {
        }

        public void Reset()
        {
            Time = 0;
        }
    }
}
