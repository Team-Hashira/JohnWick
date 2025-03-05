using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class IncreaseMoveSpeed : Effect, ICoolTimeEffect
    {
        private StatElement _speedStatElement;

        public float Duration { get; set; } = 5;
        public float Time { get; set; }

        public override int MaxActiveCount => 4;

        public override void Enable()
        {
            base.Enable();
            _speedStatElement = entityStat.StatDictionary["Speed"];
            _speedStatElement.AddModify("IncreaseMoveSpeed", 30.0f, EModifyMode.Percent);
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
            _speedStatElement.RemoveModify("IncreaseMoveSpeed");
            //TODO 여기에 이펙트
        }

        public void OnTimeOut()
        {
        }
    }
}
