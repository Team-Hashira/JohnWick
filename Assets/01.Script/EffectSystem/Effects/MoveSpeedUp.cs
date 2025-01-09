using Hashira.Core.StatSystem;

namespace Hashira.EffectSystem.Effects
{
    public class MoveSpeedUp : Effect
    {
        private StatElement _speedStatElement;
        public override void Enable()
        {
            base.Enable();
            _speedStatElement = _playerStat.GetElement("Speed");
            _speedStatElement.AddModify("MoveSpeedUp", 10.0f * level, EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Disable()
        {
            base.Disable();
            _speedStatElement.RemoveModify("MoveSpeedUp", EModifyMode.Percnet);
            //TODO 여기에 이펙트
        }
    }
}
