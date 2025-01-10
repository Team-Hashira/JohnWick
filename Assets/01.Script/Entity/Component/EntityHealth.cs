using Hashira.Core.StatSystem;
using System;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDamageable, IRecoverable
    {
        public int Health { get; private set; }

        [SerializeField] private StatElementSO _healthStatSO;

        private Entity _owner;
        private StatElement _maxHealth;
        private bool _isInvincible;
        private bool _isDie;

        public int MaxHealth => _maxHealth.IntValue;
        public event Action<int, int> OnHealthChangedEvent;
        public event Action OnDieEvent;

        public void Initialize(Entity entity)
        {
            _owner = entity;
        }

        public void AfterInit()
        {
            _maxHealth = _owner.GetEntityComponent<EntityStat>().GetElement(_healthStatSO);
            _isInvincible = _maxHealth == null;
            Health = MaxHealth;
        }

        public EEntityPartType ApplyDamage(int damage, RaycastHit2D raycastHit, Transform attackerTrm)
        {
            EEntityPartType hitPoint 
                = _owner.GetEntityComponent<EntityPartCollider>().Hit(raycastHit.collider, raycastHit, attackerTrm);
            if (_isDie) return hitPoint;

            int prev = Health;
            Health -= hitPoint == EEntityPartType.Head ? damage * 5 : damage;
            if (Health < 0)
                Health = 0;
            OnHealthChangedEvent?.Invoke(prev, Health);

            if (Health == 0) Die();

            return hitPoint;
        }

        public void ApplyRecovery(int recovery)
        {
            if (_isDie) return;

            int prev = Health;
            Health += recovery;
            if (Health > MaxHealth)
                Health = MaxHealth;
            OnHealthChangedEvent?.Invoke(prev, Health);
        }

        public void Resurrection()
        {
            _isDie = false;
            ApplyRecovery(MaxHealth);
        }

        public void Die()
        {
            _isDie = true;
            OnDieEvent?.Invoke();
        }
    }
}