using Hashira.Core.StatSystem;

namespace Hashira.EffectSystem.Effects
{
    public class IncreaseJumpCount : Effect
    {
        private StatElement _speedStatElement;

        public override void Enable()
        {
            base.Enable();
            _speedStatElement = _playerStat.GetElement("JumpCount");
            _speedStatElement.AddModify("IncreaseJumpCount", level, EModifyMode.Add);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Disable()
        {
            base.Disable();
            _speedStatElement.RemoveModify("IncreaseJumpCount", EModifyMode.Add);
        }
    }
}
