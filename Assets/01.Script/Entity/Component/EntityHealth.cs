using Crogen.CrogenPooling;
using Crogen.AttributeExtension;
using Hashira.Core.StatSystem;
using System;
using UnityEngine;
using Hashira.Entities.Components;
using Hashira.Core;
using System.Collections.Generic;
using Hashira.Core.DamageHandler;

namespace Hashira.Entities
{
    public enum EAttackType
    {
        Default,
        HeadShot,
        Fixed,
        Fire,
        Electricity,
    }

    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IDamageable, IRecoverable
    {
        public int Health { get; private set; }
        public int Shield { get; set; }

        [SerializeField] private StatElementSO _healthStatSO;

        public Entity Owner { get; private set; }
        private StatElement _maxHealth;
        private bool _isInvincible;
        public bool IsDie { get; private set; }

        public int MaxHealth => _maxHealth.IntValue;
        public event Action<int, int> OnHealthChangedEvent;
        public event Action OnHitedEvent;
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

        private Dictionary<EDamageHandlerLayer, List<DamageHandler>> _damageHandlerDict;

        public void Initialize(Entity entity)
        {
            Owner = entity;
            _damageHandlerDict = new Dictionary<EDamageHandlerLayer, List<DamageHandler>>();
            foreach (EDamageHandlerLayer layerEnum in Enum.GetValues(typeof(EDamageHandlerLayer)))
            {
                _damageHandlerDict.Add(layerEnum, new List<DamageHandler>());
            }
        }

        public void AfterInit()
        {
            _maxHealth = Owner.GetEntityComponent<EntityStat>().StatDictionary[_healthStatSO];
            _entityMover = Owner.GetEntityComponent<EntityMover>(true);
            _entityStateMachine = Owner.GetEntityComponent<EntityStateMachine>();

            _isInvincible = _maxHealth == null;
            Health = MaxHealth;
        }

        public EEntityPartType ApplyDamage(int damage, RaycastHit2D raycastHit, Transform attackerTrm, Vector2 knockback = default, EAttackType attackType = EAttackType.Default)

        {
            EEntityPartType hitPoint = EEntityPartType.Body;
            if (raycastHit != default && Owner.TryGetEntityComponent(out EntityPartCollider entityPartCollider))
            {
                hitPoint = entityPartCollider.Hit(raycastHit.collider, raycastHit, attackerTrm);
                if (hitPoint == EEntityPartType.Head && attackType == EAttackType.Default) attackType = EAttackType.HeadShot;
            }

            if (IsDie) return hitPoint;

            int prev = Health;
            int finalDamage = CalculateDamage(damage, hitPoint, attackType);

            Vector3 textPos = raycastHit != default ? raycastHit.point : Owner.transform.position;
            CreateDamageText(finalDamage, textPos, attackType);

            Health -= finalDamage;
            if (Health < 0)
                Health = 0;
            OnHealthChangedEvent?.Invoke(prev, Health);

            OnKnockback(knockback.normalized, knockback.magnitude);

            if (Health == 0) Die();

            return hitPoint;
        }
        private void CreateDamageText(int damage, Vector3 textPos, EAttackType attackType)
        {
            Color color = EnumUtility.AttackTypeColorDict[attackType];

            DamageText damageText = gameObject.Pop(UIPoolType.DamageText, textPos, Quaternion.identity).gameObject.GetComponent<DamageText>();
            damageText.Init(damage, color);
        }

        protected virtual int CalculateDamage(int damage, EEntityPartType entityPartType, EAttackType attackType)
        {
            //int finalDamage = attackType == EAttackType.HeadShot ? damage * 2 : damage;
            int finalDamage = damage;
            for (int i = 0; i < DamageHandlerOrder.OrderList.Count; i++)
            {
                List<DamageHandler> handler = _damageHandlerDict[DamageHandlerOrder.OrderList[i]];
                EDamageHandlerStatus status = CalculateDamageHandlerList(finalDamage, entityPartType, attackType, handler, out finalDamage);
                if (status == EDamageHandlerStatus.Stop)
                    break;
            }
            return finalDamage;
        }

        public EDamageHandlerStatus CalculateDamageHandlerList(int damage, EEntityPartType entityPartType, EAttackType attackType, List<DamageHandler> handlerList, out int finalDamage)
        {
            finalDamage = damage;
            foreach (DamageHandler handler in handlerList)
            {
                EDamageHandlerStatus status = handler.Calculate(damage, entityPartType, attackType, out finalDamage);
                if (status == EDamageHandlerStatus.Stop)
                    return EDamageHandlerStatus.Stop;
            }
            return EDamageHandlerStatus.Continue;
        }

        public void ApplyRecovery(int recovery)
        {
            if (IsDie) return;

            int prev = Health;
            Health += recovery;
            if (Health > MaxHealth)
                Health = MaxHealth;
            OnHealthChangedEvent?.Invoke(prev, Health);
        }

        public void Resurrection()
        {
            IsDie = false;
            ApplyRecovery(MaxHealth);
        }

        public void Die()
        {
            IsDie = true;
            OnDieEvent?.Invoke();
            OnDieEvent = null;
        }

        private void Update()
        {
            if (IsKnockback)
            {
                _entityMover.SetMovement(_knockbackDirectionX, true);

                _currentknockbackTime += Time.deltaTime;
                if (_currentknockbackTime > knockbackTime)
                {
                    _currentknockbackTime = 0;
                    IsKnockback = false;
                    _entityMover.isManualMove = true;
                    _entityMover.StopImmediately();
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

        public void AddDamageHandler(EDamageHandlerLayer layer, DamageHandler handler)
        {
            if (_damageHandlerDict[layer] == null)
                _damageHandlerDict[layer] = new List<DamageHandler>();
            _damageHandlerDict[layer].Add(handler);
        }

        public void RemoveDamageHandler(DamageHandler handler)
        {
            foreach (var list in _damageHandlerDict.Values)
            {
                bool isRemoved = list.Remove(handler);
                if (isRemoved)
                    break;
            }
        }
    }
}