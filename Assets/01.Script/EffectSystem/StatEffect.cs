using Hashira.Core.StatSystem;
using Hashira.Entities;

namespace Hashira.EffectSystem
{
    public class StatEffect : Effect
    {
        private StatDictionary _statDictionary;
        private StatElementSO _statSO;
        private StatModifier _statModifier;
        private string _key;

        public void Init(StatElementSO statSO, StatModifier statModifier)
        {
            _statSO = statSO;
            _statModifier = statModifier;
            _key = $"{_statSO.statName}{_statModifier.Value}StatEffect";
        }

        public override void Enable()
        {
            base.Enable();
            _statDictionary = entityEffector.Entity.GetEntityComponent<EntityStat>().StatDictionary;
            _statDictionary[_statSO].AddModify(_key, _statModifier);
        }

        public override void Disable()
        {
            base.Disable();
            _statDictionary[_statSO].RemoveModify(_key);
        }
    }
}
