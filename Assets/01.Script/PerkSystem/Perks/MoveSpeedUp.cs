using Hashira.Core.StatSystem;
using Hashira.EffectSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.PerkSystem.Perks
{
    public class MoveSpeedUp : Perk
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
                EffectManager.Instance.RemoveEffect<EffectSystem.Effects.IncreaseMoveSpeed>(_playerEffector);
            }
        }

        public override void UsePerk()
        {
            base.UsePerk();
            if (_stackCount > _maxStackCount) return;
            ++_stackCount;
            EffectManager.Instance.AddEffect<EffectSystem.Effects.IncreaseMoveSpeed>(_playerEffector, _stackCount, 2f);
        }
    }
}