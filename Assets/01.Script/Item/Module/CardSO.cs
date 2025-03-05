using Hashira.EffectSystem;
using Hashira.Projectiles;
using System;
using UnityEngine;

namespace Hashira.Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "SO/Card")]
    public class CardSO : ScriptableObject
    {
        [Header("==========Card setting==========")]

        public string iconSprite;
        public string cardName;
        public string cardDescription;
        public int cost;

        public string effectClassName;
        [SerializeReference] private Type _effectType;

        protected virtual void OnValidate()
        {
            if (_effectType != null && _effectType.Name == effectClassName) return;

            string className = $"{typeof(Effect).Namespace}.{effectClassName}";
            try
            {
                _effectType = Type.GetType(className);
            }
            catch (Exception e)
            {
                Debug.LogError($"{className} is not found.\n" +
                    $"Error : {e.ToString()}");
            }
        }

        public Effect GetEffectClass()
            => Activator.CreateInstance(_effectType) as Effect;
    }

    //지울꺼
    [Serializable]
    public class ProjectileModifierSetting
    {
        public ProjectileModifierSO projectileModifierSO;
        [SerializeReference] public ProjectileModifier projectileModifier;
        //public ExecutionConditionSetting conditionSetting;
    }
}
