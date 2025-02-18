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
        private Dictionary<Entity, List<Effect>>[] _effectDictionaries = new Dictionary<Entity, List<Effect>>[10];

        public event Action<Effect> EffectAddedEvent;
        public event Action<Effect> EffectRemovedEvent;

        private void Awake()
        {
            for (int i = 0; i < 10; i++)
            {
                _effectDictionaries[i] = new Dictionary<Entity, List<Effect>>();
            }
        }

        public void AddEffect<T>(Entity entity, int level) where T : Effect, new()
        {
            T effect = new T()
            {
                name = typeof(T).Name,
                level = level,
                effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == typeof(T).Name),
                entity = entity,
				entityStat = entity.GetEntityComponent<EntityStat>()
            };

            _effectDictionaries[level].TryAdd(entity, new List<Effect>());

            _effectDictionaries[level][entity].Add(effect);
            
			effect.Enable();
            EffectAddedEvent?.Invoke(effect);
		}

		public void RemoveEffect<T>(Entity entity, int level) where T : Effect
        {
            Effect removeEffect = _effectDictionaries[level][entity].FirstOrDefault(x=>x.name==typeof(T).Name);

            if (removeEffect != null)
            {
                removeEffect.Disable();
                EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionaries[level][entity].Remove(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");
        }

		public void RemoveEffect(Entity entity, Type effectType, int level)
        {
            Effect removeEffect = _effectDictionaries[level][entity].FirstOrDefault(x=>x.name==effectType.Name);

            if (removeEffect != null)
            {
			    removeEffect.Disable();
                EffectRemovedEvent?.Invoke(removeEffect);
                _effectDictionaries[level][entity].Remove(removeEffect);
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
            for (int level = 0; level < _effectDictionaries.Length; level++)
            {
                foreach (var effectList in _effectDictionaries[level])
                {
                    if (effectList.Key == null)
                    {
                        InitEffectDict(level);
                        return;
                    }

                    foreach (var effect in effectList.Value)
                    {
                        effect?.Update();
                        EffectInterfaceLogic(effect);
                    }
                }
            }
        }

        private void InitEffectDict(int level)
        {
            _effectDictionaries[level] = _effectDictionaries[level]
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

            RemoveEffect(effect.entity, effect.GetType(), effect.level);
        }
    }
}
