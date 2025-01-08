using System;
using System.Collections.Generic;
using System.Linq;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.EffectSystem
{
    public class EffectManager : MonoSingleton<EffectManager>
    {
        [SerializeField] private List<EffectUIDataSO> _effectUIDataSOList = new List<EffectUIDataSO>();
        private readonly List<Effect> _effectList = new List<Effect>();

        public event Action<Effect> EffectAddedEvent;
        public event Action<Effect> EffectRemovedEvent;

        public void AddEffect<T>(EntityEffector baseEffector, int level, float duration) where T : Effect, new()
        {
            T effect = new T()
            {
                name = typeof(T).Name,
                duration = duration,
                level = level,
                effectUIDataSO = _effectUIDataSOList.FirstOrDefault(x=>x.name == typeof(T).Name)
            };
            
            _effectList.Add(effect);
            
            EffectAddedEvent?.Invoke(effect);
        }

        public void RemoveEffect<T>(EntityEffector baseEffector) where T : Effect
        {
            Effect removeEffect = _effectList.FirstOrDefault(x=>x.name==typeof(T).Name);

            if (removeEffect != null)
            {
                _effectList.Remove(removeEffect);
                
                EffectRemovedEvent?.Invoke(removeEffect);
            }
            else
                Debug.Log($"Effect {typeof(T).Name} was not found");
        }
        
        private void Update()
        {
            foreach (var effect in _effectList)
            {
                effect.Update();
            }
        }
    }
}
