using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace Hashira.Core.StatSystem
{
    public enum EModifyMode
    {
        Add,
        Percnet
    }

    public struct StatModifier
    {
        private float _originValue;
        private bool _canValueOverlap;

        public int _overlapCount;
        public EModifyMode Mode { get; private set; }
        public float Value { get; private set; }

        public StatModifier(float originValue, EModifyMode mode, bool canValueOverlap)
        {
            _originValue = originValue;
            Mode = mode;
            _overlapCount = 1;
            _canValueOverlap = canValueOverlap;

            Value = originValue;
        }

        public static StatModifier operator ++(StatModifier modifier)
        {
            modifier._overlapCount++;
            if (modifier._canValueOverlap)
                modifier.Value += modifier._originValue;
            return modifier;
        }
        public static StatModifier operator --(StatModifier modifier)
        {
            modifier._overlapCount--;
            if (modifier._canValueOverlap)
                modifier.Value -= modifier._originValue;
            return modifier;
        }
        public static implicit operator int(StatModifier modifier)
        {
            return modifier._overlapCount;
        }
    }

    [Serializable]
    public class StatElement : ICloneable
    {
        [HideInInspector] public string Name;
        public StatElementSO elementSO;
        [SerializeField] private float _baseValue;
        private Dictionary<string, StatModifier> _modifiers;

        public event Action<float, float> OnValueChanged;
        public event Action<int, int> OnIntValueChanged;

        public float Value { get; private set; }
        public int IntValue { get; private set; }

        private bool _isUseClamp;
        private bool _isUseModifier;

        public void Initialize(bool isUseClamp = true, bool isUseModifier = true)
        {
            _isUseClamp = isUseClamp;
            _isUseModifier = isUseModifier;
            
            SetDictionary();
            SetValue();
        }

        private void SetDictionary()
        {
            _modifiers ??= new Dictionary<string, StatModifier>();
        }

        private void SetValue()
        {
            float totalAddModifier = 0;
            float totalPercentModifier = 0;
            if (_isUseModifier)
            {
                foreach (StatModifier modifier in _modifiers.Values)
                {
                    if (modifier.Mode == EModifyMode.Add)
                        totalAddModifier += modifier.Value;
                    else
                        totalPercentModifier += modifier.Value;
                }
            }

            float value = (_baseValue + totalAddModifier) * (1 + totalPercentModifier / 100);

            if (elementSO != null && _isUseClamp)
                value = Mathf.Clamp(value, elementSO.minMaxValue.x, elementSO.minMaxValue.y);

            int intValue = Mathf.CeilToInt(value);

            if (Value != value) OnValueChanged?.Invoke(Value, value);
            if (IntValue != intValue) OnIntValueChanged?.Invoke(IntValue, intValue);

            Value = value;
            IntValue = intValue;
        }

        public void AddModify(string key, float value, EModifyMode eModifyMode, bool canValueOverlap = true)
        {
            if (_modifiers.ContainsKey(key))
                _modifiers[key]++;
            else
            {
                StatModifier modifier = new StatModifier(value, eModifyMode, canValueOverlap);
                _modifiers[key] = modifier;
            }

            SetValue();
        }
        public void RemoveModify(string key, EModifyMode eModifyMode)
        {
            if (_modifiers.ContainsKey(key))
            {
                _modifiers[key]--;
                if (_modifiers[key] == 0)
                    _modifiers.Remove(key);
                SetValue();
            }
            else
                Debug.LogWarning($"[{key}]Key not found for statModifier");
        }

        public object Clone()
        {
            StatElement clonedStatElement = (StatElement)MemberwiseClone();
            clonedStatElement._modifiers = new Dictionary<string, StatModifier>();
            return clonedStatElement;
        }
    }
}