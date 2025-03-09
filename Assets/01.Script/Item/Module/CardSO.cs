using Hashira.EffectSystem;
using Hashira.Projectiles;
using System;
using UnityEngine;

namespace Hashira.Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "SO/Card")]
    public class CardSO : ScriptableObject
    {
        [Header("==========CardSO==========")]

        public Sprite cardSprite;
        public string cardDisplayName;
        public string cardName;
        [TextArea]
        public string cardDescription;
        public int needCost;

        public string effectClassName;
        private Type _effectType;

        [Doryu.CustomAttributes.Uncorrectable]
        [SerializeField] private string _effectTypeIs;

        protected virtual void OnValidate()
        {
            if (_effectType != null && _effectType.Name == effectClassName) return;

            string className = $"{typeof(Effect).Namespace}.Effects.{effectClassName}";
            try
            {
                _effectType = Type.GetType(className);
            }
            catch (Exception e)
            {
                Debug.LogError($"{className} is not found.\n" +
                    $"Error : {e.ToString()}");
            }

            _effectTypeIs = _effectType?.Name;
        }

        public virtual Effect GetEffectClass()
        {
            Effect effect = Activator.CreateInstance(_effectType) as Effect;
            effect.SetCardSO(this);
            return effect;
        }
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
