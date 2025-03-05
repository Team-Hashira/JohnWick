using Hashira.Core.StatSystem;
using Hashira.EffectSystem;
using UnityEngine;

namespace Hashira.Cards
{
    [CreateAssetMenu(fileName = "StatCard", menuName = "SO/Card/StatCard")]
    public class StatCardSO : CardSO
    {
        [Header("==========StatCardSO==========")]
        public StatElementSO statSO;
        public StatModifier statModifier;

        public override Effect GetEffectClass()
        {
            Effect effect = base.GetEffectClass();
            if (effect is StatEffect statEffect)
            {
                statEffect.Init(statSO, statModifier);
                return statEffect;
            }
            else
            {
                Debug.LogWarning("this Effect is not StatEffect.");
                return effect;
            }
        }

        protected override void OnValidate()
        {
            effectClassName = "StatEffect";
            base.OnValidate();
        }
    }
}
