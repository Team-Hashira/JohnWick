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

        public void AddEffect<T>(Entity entity, int level, float duration) where T : Effect, new()
        {
            T effect = new T()
            {
                name = typeof(T).Name,
                duration = duration,
                level = level,
                effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x => x.name == typeof(T).Name),
                entity = entity,
				entityStat = entity.GetEntityComponent<EntityStat>()
            };

            _effectDictionaries[level].TryAdd(entity, new List<Effect>());

            _effectDictionaries[level][entity].Add(effect);
            
            EffectAddedEvent?.Invoke(effect);
			effect.Enable();
		}

		public void RemoveEffect<T>(Entity entity, int level) where T : Effect
        {
            Effect removeEffect = _effectDictionaries[level][entity].FirstOrDefault(x=>x.name==typeof(T).Name);

            if (removeEffect != null)
            {
                _effectDictionaries[level][entity].Remove(removeEffect);
                
                EffectRemovedEvent?.Invoke(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");

			removeEffect.Disable();
		}

		public void RemoveEffect(Entity entity, Type effectType, int level)
        {
            Effect removeEffect = _effectDictionaries[level][entity].FirstOrDefault(x=>x.name==effectType.Name);

            if (removeEffect != null)
            {
                _effectDictionaries[level][entity].Remove(removeEffect);
                
                EffectRemovedEvent?.Invoke(removeEffect);
            }
            else
                Debug.Log($"Effect {effectType.Name} was not found");

			removeEffect.Disable();
		}

        private void InitEffectDict(int level)
        {
			_effectDictionaries[level] = _effectDictionaries[level].Where(x => x.Key != null).ToDictionary(x=>x.Key, x=>x.Value);
		}

		private void Update()
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
						effect.Update();

						if (effect.duration < 0) continue;

						effect.currentTime += Time.deltaTime;
						if (effect.currentTime >= effect.duration)
						{
							RemoveEffect(effect.entity, effect.GetType(), level);
						}
					}
				}
			}
        }
    }
}
