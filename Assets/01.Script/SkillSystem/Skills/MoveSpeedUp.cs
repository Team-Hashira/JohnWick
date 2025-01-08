using System;
using System.Collections;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.SkillSystem.Skills
{
    public class MoveSpeedUp : Skill
    {
        private Player _player;
        private EntityEffector _playerEffector;
        private EntityHealth _health;
        private StatElement _statElement;
        
        [SerializeField] private int _maxStackCount = 2;
        [SerializeField] private int _stackCount = 0;
        
        private void Awake()
        {
            _player = GameManager.Instance.Player;
        }

        private void Start()
        {
            _statElement = _player.GetEntityComponent<EntityStat>().GetElement("Speed");
            _playerEffector = _player.GetEntityComponent<EntityEffector>();
            _health = _player.GetEntityComponent<EntityHealth>();
            _health.OnHealthChangedEvent += HandleStackCountReset;
        }

        private void OnDestroy()
        {
            _health.OnHealthChangedEvent -= HandleStackCountReset;
        }

        private void HandleStackCountReset(int lastValue, int newValue)
        {
            if (lastValue > newValue)
            {
                int prev = _stackCount;
                
                EffectManager.Instance.RemoveEffect<EffectSystem.Effects.MoveSpeedUp>(_playerEffector);
                
                Debug.Log(_stackCount);
            }
        }

        public override void UseSkill()
        {
            base.UseSkill();
            if (_stackCount > _maxStackCount) return;
            ++_stackCount;
            EffectManager.Instance.AddEffect<EffectSystem.Effects.MoveSpeedUp>(_playerEffector, _stackCount, 4f);
        }
    }
}