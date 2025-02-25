using System;
using System.Collections.Generic;
using System.Linq;
using Hashira.EffectSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.UI.Effect
{
    public class EffectContainer : MonoBehaviour
    {
        [SerializeField] private EffectSlot _effectSlotPrefab;
        private readonly List<EffectSlot> _currentSlots = new List<EffectSlot>();

        private EffectManager _effectManager;

        private EntityEffector _playerEntityEffector;

        private void Awake()
        {
            _effectManager = EffectManager.Instance;
        }

        private void Start()
        {
            _playerEntityEffector = GameManager.Instance.Player.GetEntityComponent<EntityEffector>();

            _playerEntityEffector.EffectAddedEvent += AddEffectUI;
            _playerEntityEffector.EffectRemovedEvent += RemoveEffectUI;
        }

        private void OnDestroy()
        {
            _playerEntityEffector.EffectAddedEvent -= AddEffectUI;
            _playerEntityEffector.EffectRemovedEvent -= RemoveEffectUI;
        }

        private void AddEffectUI(EffectSystem.Effect effect)
        {
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
