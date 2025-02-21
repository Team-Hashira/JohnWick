using System;
using System.Collections.Generic;
using System.Linq;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hashira.EffectSystem
{
    public class EffectManager : MonoSingleton<EffectManager>
    {
        [SerializeField] private List<EffectUIDataSO> _effectUIDataSOList = new List<EffectUIDataSO>();
        private Dictionary<EntityEffector, Dictionary<Type, List<Effect>>> _effectDictionary = new Dictionary<EntityEffector, Dictionary<Type, List<Effect>>>();

        public void AddEffect<T>(EntityEffector entityEffector) where T : Effect, new()
        {
            T effect = new T()
            {
                name = typeof(T).Name,
                effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == typeof(T).Name),
                entityEffector = entityEffector,
                entityStat = entityEffector.Entity.GetEntityComponent<EntityStat>()
            };

            _effectDictionary.TryAdd(entityEffector, new Dictionary<Type, List<Effect>>());
            _effectDictionary[entityEffector].TryAdd(typeof(T), new List<Effect>());

            if (IsCanAddEffect(entityEffector, effect) == false) return;

            _effectDictionary[entityEffector][typeof(T)].Add(effect);

            effect.Enable();
            entityEffector.EffectAddedEvent?.Invoke(effect);
        }
        public void AddEffect<T>(EntityEffector entityEffector, T effect) where T : Effect
        {
            Type type = typeof(T);
            effect.name = type.Name;
            effect.effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == type.Name);
            effect.entityEffector = entityEffector;
            effect.entityStat = entityEffector.Entity.GetEntityComponent<EntityStat>();

            _effectDictionary.TryAdd(entityEffector, new Dictionary<Type, List<Effect>>());
            _effectDictionary[entityEffector].TryAdd(type, new List<Effect>());

            if (IsCanAddEffect(entityEffector, effect) == false) return;

            _effectDictionary[entityEffector][type].Add(effect);

            effect.Enable();
            entityEffector.EffectAddedEvent?.Invoke(effect);
        }

        public void AddEffect<T>(Entity entity) where T : Effect, new()
        {
            EntityEffector entityEffector = entity.GetEntityComponent<EntityEffector>();
            Debug.Assert(entityEffector != null, "EntityEffector is null!");

            T effect = new T()
            {
                name = typeof(T).Name,
                effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == typeof(T).Name),
                entityEffector = entityEffector,
                entityStat = entityEffector.Entity.GetEntityComponent<EntityStat>()
            };

            _effectDictionary.TryAdd(entityEffector, new Dictionary<Type, List<Effect>>());
            _effectDictionary[entityEffector].TryAdd(typeof(T), new List<Effect>());

            if (IsCanAddEffect(entityEffector, effect) == false) return;

            _effectDictionary[entityEffector][typeof(T)].Add(effect);

            effect.Enable();
            entityEffector.EffectAddedEvent?.Invoke(effect);
        }
        public void AddEffect(Entity entity, Effect effect)
        {
            EntityEffector entityEffector = entity.GetEntityComponent<EntityEffector>();
            Debug.Assert(entityEffector != null, "EntityEffector is null!");

            Type type = effect.GetType();
            effect.name = type.Name;
            effect.effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == type.Name);
            effect.entityEffector = entityEffector;
            effect.entityStat = entityEffector.Entity.GetEntityComponent<EntityStat>();

            _effectDictionary.TryAdd(entityEffector, new Dictionary<Type, List<Effect>>());
            _effectDictionary[entityEffector].TryAdd(type, new List<Effect>());

            if (IsCanAddEffect(entityEffector, effect) == false) return;

            _effectDictionary[entityEffector][type].Add(effect);

            effect.Enable();
            entityEffector.EffectAddedEvent?.Invoke(effect);
        }

        public void RemoveEffect<T>(EntityEffector entityEffector) where T : Effect
        {
            Effect removeEffect = _effectDictionary[entityEffector][typeof(T)].FirstOrDefault(x=>x.name==typeof(T).Name);

            if (removeEffect != null)
            {
                removeEffect.Disable();
                entityEffector.EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[entityEffector][typeof(T)].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");
        }
		public void RemoveEffect(EntityEffector entityEffector, Type effectType)
        {
            Effect removeEffect = _effectDictionary[entityEffector][effectType].FirstOrDefault(x=>x.name==effectType.Name);

            if (removeEffect != null)
            {
			    removeEffect.Disable();
                entityEffector.EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[entityEffector][effectType].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {effectType.Name} was not found");
        }
        public void RemoveEffect(EntityEffector entityEffector, Effect effect)
        {
            Type type = effect.GetType();

            if (effect != null)
            {
                effect.Disable();
                entityEffector.EffectRemovedEvent?.Invoke(effect);
                _effectDictionary[entityEffector][type].Remove(effect);
            }
            else
                Debug.Log($"Effect {type.Name} was not found");
        }

        public void RemoveEffect<T>(Entity entity) where T : Effect
        {
            EntityEffector entityEffector = entity.GetEntityComponent<EntityEffector>();
            Debug.Assert(entityEffector != null, "EntityEffector is null!");

            Effect removeEffect = _effectDictionary[entityEffector][typeof(T)].FirstOrDefault(x => x.name == typeof(T).Name);

            if (removeEffect != null)
            {
                removeEffect.Disable();
                entityEffector.EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[entityEffector][typeof(T)].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");
        }
        public void RemoveEffect(Entity entity, Type effectType)
        {
            EntityEffector entityEffector = entity.GetEntityComponent<EntityEffector>();
            Debug.Assert(entityEffector != null, "EntityEffector is null!");

            Effect removeEffect = _effectDictionary[entityEffector][effectType].FirstOrDefault(x => x.name == effectType.Name);

            if (removeEffect != null)
            {
                removeEffect.Disable();
                entityEffector.EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[entityEffector][effectType].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {effectType.Name} was not found");
        }
        public void RemoveEffect(Entity entity, Effect effect)
        {
            EntityEffector entityEffector = entity.GetEntityComponent<EntityEffector>();
            Debug.Assert(entityEffector != null, "EntityEffector is null!");

            Type type = effect.GetType();

            if (effect != null)
            {
                effect.Disable();
                entityEffector.EffectRemovedEvent?.Invoke(effect);
                _effectDictionary[entityEffector][type].Remove(effect);
            }
            else
                Debug.Log($"Effect {type.Name} was not found");
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                AddEffect<IncreaseMoveSpeed>(GameManager.Instance.Player);
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                AddEffect<IncreaseMoveSpeed>(GameManager.Instance.Player);
            }

            UpdateEffects();
        }

        private void UpdateEffects()
        {
            foreach (var effectList in _effectDictionary)
            {
                // Entity가 죽었을 때 체크
                if (effectList.Key == null)
                {
                    InitEffectDict();
                    return;
                }

                foreach (var effectPair in effectList.Value)
                {
                    for (var j = 0; j < effectPair.Value.Count; j++)
                    {
                        var effect = effectPair.Value[j];
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
        }

        private void InitEffectDict()
        {
            _effectDictionary = _effectDictionary
                .Where(x => x.Key != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private bool IsCanAddEffect<T>(EntityEffector entityEffector, T effect) where T : Effect
        {
            if (effect.MaxActiveCount < 0) return true;
            return effect.MaxActiveCount > _effectDictionary[entityEffector][typeof(T)].Count;
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

            RemoveEffect(effect.entityEffector, effect.GetType());
        }
    }
}
