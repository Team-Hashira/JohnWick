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
        private readonly Dictionary<Entity, List<Effect>> _effectDict = new Dictionary<Entity, List<Effect>>();

        public event Action<Effect> EffectAddedEvent;
        public event Action<Effect> EffectRemovedEvent;

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

            _effectDict.TryAdd(entity, new List<Effect>());

            _effectDict[entity].Add(effect);
            
            EffectAddedEvent?.Invoke(effect);
			effect.Enable();
		}

		public void RemoveEffect<T>(Entity entity) where T : Effect
        {
            Effect removeEffect = _effectDict[entity].FirstOrDefault(x=>x.name==typeof(T).Name);

            if (removeEffect != null)
            {
                _effectDict[entity].Remove(removeEffect);
                
                EffectRemovedEvent?.Invoke(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");

			removeEffect.Disable();
		}

		public void RemoveEffect(Entity entity, Type effectType)
        {
            Effect removeEffect = _effectDict[entity].FirstOrDefault(x=>x.name==effectType.Name);

            if (removeEffect != null)
            {
                _effectDict[entity].Remove(removeEffect);
                
                EffectRemovedEvent?.Invoke(removeEffect);
            }
            else
                Debug.Log($"Effect {effectType.Name} was not found");

			removeEffect.Disable();
		}

		private void Update()
        {
            foreach (var effectList in _effectDict)
            {
				for (int i = 0; i < effectList.Value.Count; i++)
				{
					var effect = effectList.Value[i];

					effect.Update();
					effect.currentTime += Time.deltaTime;
					if (effect.currentTime >= effect.duration)
					{
						RemoveEffect(effect.entity, effect.GetType());
					}
				}
			}
        }
    }
}
