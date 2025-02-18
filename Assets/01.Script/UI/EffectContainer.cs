using System;
using System.Collections.Generic;
using System.Linq;
using Hashira.EffectSystem;
using UnityEngine;

namespace Hashira.UI.Effect
{
    public class EffectContainer : MonoBehaviour
    {
        [SerializeField] private EffectSlot _effectSlotPrefab;
        private readonly List<EffectSlot> _currentSlots = new List<EffectSlot>();

        private EffectManager _effectManager;

        private void Awake()
        {
            _effectManager = EffectManager.Instance;
        }

        private void Start()
        {
            _effectManager.EffectAddedEvent += AddEffectUI;
            _effectManager.EffectRemovedEvent += RemoveEffectUI;
        }

        private void OnDestroy()
        {
            _effectManager.EffectAddedEvent -= AddEffectUI;
            _effectManager.EffectRemovedEvent -= RemoveEffectUI;
        }

        private void AddEffectUI(EffectSystem.Effect effect)
        {
            EffectSlot oldEffectSlot = _currentSlots.FirstOrDefault(x => x.Equals(effect));

            //만약 같은 종류의 효과를 만나면
            if (oldEffectSlot != null)
            {
                oldEffectSlot.effectBase = effect;
                return;
            }
            
            EffectSlot effectSlot = Instantiate(_effectSlotPrefab, transform);
            effectSlot.Init(effect);
            _currentSlots.Add(effectSlot);   
        }
        
        private void RemoveEffectUI(EffectSystem.Effect effect)
        {
            EffectSlot effectSlot = _currentSlots.FirstOrDefault(x => x.Equals(effect));

            if (effectSlot != null)
            {
                _currentSlots.Remove(effectSlot);
                Destroy(effectSlot.gameObject);
            }
            else
                Debug.Log($"Effect {effect.name} was not found");
        }
    }
}
