using System;
using System.Collections.Generic;
using System.Linq;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public class EffectManager : MonoSingleton<EffectManager>
    {
        [SerializeField] private List<EffectUIDataSO> _effectUIDataSOList = new List<EffectUIDataSO>();
        private Dictionary<Entity, Dictionary<Type, List<Effect>>> _effectDictionary = new Dictionary<Entity, Dictionary<Type, List<Effect>>>();

        public event Action<Effect> EffectAddedEvent;
        public event Action<Effect> EffectRemovedEvent;

        public void AddEffect<T>(Entity entity, int level) where T : Effect, new()
        {
            T effect = new T()
            {
                name = typeof(T).Name,
                effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == typeof(T).Name),
                entity = entity,
				entityStat = entity.GetEntityComponent<EntityStat>()
            };

            _effectDictionary.TryAdd(entity, new Dictionary<Type, List<Effect>>());
            _effectDictionary[entity].TryAdd(typeof(T), new List<Effect>());

            if (IsCanAddEffect(_effectDictionary[entity][typeof(T)]) == false) return;

            _effectDictionary[entity][typeof(T)].Add(effect);
            
			effect.Enable();
            EffectAddedEvent?.Invoke(effect);
		}

		public void RemoveEffect<T>(Entity entity) where T : Effect
        {
            Effect removeEffect = _effectDictionary[entity][typeof(T)].FirstOrDefault(x=>x.name==typeof(T).Name);

            if (removeEffect != null)
            {
                removeEffect.Disable();
                EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[entity][typeof(T)].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");
        }

		public void RemoveEffect(Entity entity, Type effectType)
        {
            Effect removeEffect = _effectDictionary[entity][effectType].FirstOrDefault(x=>x.name==effectType.Name);

            if (removeEffect != null)
            {
			    removeEffect.Disable();
                EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionary[entity][effectType].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {effectType.Name} was not found");
        }

		private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                AddEffect<IncreaseMoveSpeed>(GameManager.Instance.Player, 1);
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                AddEffect<IncreaseMoveSpeed>(GameManager.Instance.Player, 2);
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

        private bool IsCanAddEffect(List<Effect> effectList)
        {
            if (Effect.maxActiveCount < 0) return true;
            return Effect.maxActiveCount > effectList.Count;
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

            RemoveEffect(effect.entity, effect.GetType());
        }
    }
}
