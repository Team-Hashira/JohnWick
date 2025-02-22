using Hashira.Entities;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class Ignition : Effect, ICoolTimeEffect
    {
        private int _damage;
        private float _damageDelay = 0.5f;
        private float _lastDamageTime;

        public override int MaxActiveCount => -1;

        float ICoolTimeEffect.Duration { get; set; } = 5;
        float ICoolTimeEffect.Time { get; set; }

        public void Setup(int damage, float damageDelay, float duration)
        {
            _damage = damage;
            _damageDelay = damageDelay;
            (this as ICoolTimeEffect).Duration = duration;
        }

        public override void Enable()
        {
            base.Enable();
            Debug.Log("Enable");
            _lastDamageTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            Debug.Log("Update");
            if (_lastDamageTime + _damageDelay < Time.time)
            {
                _lastDamageTime = Time.time;
               entityEffector.Entity.GetEntityComponent<EntityHealth>().ApplyDamage(_damage, default, null, attackType: EAttackType.Fire);
            }
        }

        public override void Disable()
        {
            base.Disable();
            Debug.Log("Disable");
        }

        public void OnTimeOut()
        {

        }
    }
}
