using Crogen.CrogenPooling;
using Crogen.AttributeExtension;
using Hashira.Core.StatSystem;
using System;
using UnityEngine;
using Hashira.Entities.Components;

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

        private EntityMover _entityMover;
        private EntityStateMachine _entityStateMachine;

        public bool canKnockback = true;
        public bool IsEvasion { get; set; }
        public bool IsKnockback { get; private set; }
        [HideInInspectorByCondition(nameof(canKnockback))]
        public float knockbackTime = 0.2f;
        private float _currentknockbackTime = 0;
        private float _knockbackDirectionX; 

		public void Initialize(Entity entity)
        {
            _owner = entity;
        }

        public void AfterInit()
        {
            _maxHealth = _owner.GetEntityComponent<EntityStat>().StatDictionary[_healthStatSO];
			_entityMover = _owner.GetEntityComponent<EntityMover>(true);
			_entityStateMachine = _owner.GetEntityComponent<EntityStateMachine>();

			_isInvincible = _maxHealth == null;
            Health = MaxHealth;
		}

		public EEntityPartType ApplyDamage(int damage, RaycastHit2D raycastHit, Transform attackerTrm, Vector2 knockback = default, bool isFixedDamage = false)
        {
            EEntityPartType hitPoint;
            if (raycastHit != default && _owner.TryGetEntityComponent(out EntityPartCollider entityPartCollider))
                hitPoint = entityPartCollider.Hit(raycastHit.collider, raycastHit, attackerTrm);
            else
                hitPoint = EEntityPartType.Body;

            if (_isDie) return hitPoint;

            int prev = Health;
            bool isHead = hitPoint == EEntityPartType.Head;
            int finalDamage = (isHead && isFixedDamage == false) ? damage * 2 : damage;
            Vector3 textPos = raycastHit != default ? raycastHit.point : _owner.transform.position;
            DamageText damageText = gameObject.Pop(UIPoolType.DamageText, textPos, Quaternion.identity)
                                    .gameObject.GetComponent<DamageText>();
            damageText.Init(finalDamage, isHead ? Color.yellow : Color.white);
            Health -= finalDamage;
            if (Health < 0)
                Health = 0;
            OnHealthChangedEvent?.Invoke(prev, Health);

            //Vector2 attackDir = (raycastHit.transform.position - transform.position).normalized;

			OnKnockback(knockback.normalized, knockback.magnitude);

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
            OnDieEvent = null;
		}

		private void Update()
		{
			if (IsKnockback)
            {
				_entityMover.SetMovement(_knockbackDirectionX, true);

				_currentknockbackTime += Time.deltaTime;
                if( _currentknockbackTime > knockbackTime)
                {
                    _currentknockbackTime = 0;
                    IsKnockback = false;
                    _entityMover.isManualMove = true;
                }
            }
		}

		public void OnKnockback(Vector2 hitDir, float knockbackPower)
        {
            _entityMover.StopImmediately();
            _entityMover.isManualMove = false;
			_knockbackDirectionX = Mathf.Sign(hitDir.x) * (knockbackPower / _entityMover.Rigidbody2D.mass);
			_entityStateMachine.ChangeState("Hit");
            IsKnockback = true;
		}
    }
}