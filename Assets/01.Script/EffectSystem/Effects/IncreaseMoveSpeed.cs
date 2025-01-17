using Hashira.Core.StatSystem;

namespace Hashira.EffectSystem.Effects
{
    public class IncreaseMoveSpeed : Effect
    {
        private StatElement _speedStatElement;
        public override void Enable()
        {
            base.Enable();
            _speedStatElement = _playerStat.StatDictionary["Speed"];
            _speedStatElement.AddModify("IncreaseMoveSpeed", 10.0f * level, EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Disable()
        {
            base.Disable();
            _speedStatElement.RemoveModify("IncreaseMoveSpeed", EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }
    }
}
