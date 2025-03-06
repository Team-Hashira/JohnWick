using Hashira.EffectSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityEffector : MonoBehaviour, IEntityComponent
    {
        public Entity Entity { get; private set; }
        public event Action<Effect> EffectAddedEvent;
        public event Action<Effect> EffectRemovedEvent;
        Dictionary<Type, List<Effect>> _effectDictionary = new Dictionary<Type, List<Effect>>();

        public void Initialize(Entity entity)
        {
			Entity = entity;
		}

        public void AddEffect(Effect effect)
        {
            Type type = effect.GetType();
            effect.name = type.Name;
            effect.entityEffector = this;
            effect.entityStat = this.Entity.GetEntityComponent<EntityStat>();

            _effectDictionary.TryAdd(type, new List<Effect>());

            if (IsCanAddEffect(effect) == false) return;

            _effectDictionary[type].Add(effect);

            effect.Enable();
            EffectAddedEvent?.Invoke(effect);
        }
        public T AddEffect<T>() where T : Effect, new()
        {
            T effect = new T()
            {
                name = typeof(T).Name,
                entityEffector = this,
                entityStat = Entity.GetEntityComponent<EntityStat>()
            };

            AddEffect(effect);

            return effect;
        }

        private bool IsCanAddEffect(Effect effect)
        {
            if (effect.MaxActiveCount < 0) return true;
            return effect.MaxActiveCount > _effectDictionary[effect.GetType()].Count;
        }

        public void RemoveEffect(Effect effect)
        {
            Type type = effect.GetType();

            if (effect != null)
            {
                effect.Disable();
                EffectRemovedEvent?.Invoke(effect);
                _effectDictionary[type].Remove(effect);
            }
            else
                Debug.Log($"Effect {type.Name} was not found");
        }
        public void RemoveEffect(Type effectType)
        {
            Effect removeEffect = _effectDictionary[effectType].FirstOrDefault(x => x.name == effectType.Name);

            if (removeEffect != null)
            {
                removeEffect.Disable();
                EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[effectType].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {effectType.Name} was not found");
        }

        private void Update()
        {
            UpdateEffects();
        }

        private void UpdateEffects()
        {
            foreach (var effectList in _effectDictionary)
            {
                for (int i = 0; i < effectList.Value.Count; i++)
                {
                    var effect = effectList.Value[i];
                    // 버프가 수명이 다해서 없어질 때 체크
                    if (effect == null)
                    {
                        InitEffectDict();
                        return;
                    }

                    EffectInterfaceLogic(effect);
                    effect.Update();
                }
            }
        }

        private void OnDestroy()
        {
            InitEffectDict();
        }

        private void InitEffectDict()
        {
            _effectDictionary = _effectDictionary
                .Where(x => x.Key != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private void EffectInterfaceLogic(Effect effect)
        {
            if (effect is ICoolTimeEffect coolTimeEffect)
            {
                coolTimeEffect.Time += Time.deltaTime;
                if (coolTimeEffect.Time > coolTimeEffect.Duration)
                {
                    coolTimeEffect.Time = 0;
                    OnEndEffect(effect);
                }
            }

            if (effect is ICountingEffect countingEffect)
            {
                if (countingEffect.MaxCount < 0) return;
                if (countingEffect.MaxCount <= countingEffect.Count)
                {
                    countingEffect.Count = 0;
                    OnEndEffect(effect);
                }
            }
        }

        // Effect들의 초기화(죽음)
        private void OnEndEffect(Effect effect)
        {
            // ILoopEffect는 Effect의 사이클 자체를 담당하는 특별한 인터페이스이기 때문에 얘만 이렇게 따로 빼둘 필요가 있음
            if (effect is ILoopEffect loopEffect)
            {
                loopEffect.Reset();
                return;
            }

            RemoveEffect(effect);
        }
    }
}
