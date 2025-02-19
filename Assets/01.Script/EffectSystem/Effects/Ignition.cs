using Hashira.Entities;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class Ignition : Effect, ICoolTimeEffect
    {
        public int damage;
        private float _damageDelay = 0.5f;
        private float _lastDamageTime;

        float ICoolTimeEffect.Duration { get; set; } = 5;
        float ICoolTimeEffect.Time { get; set; }

        public override void Enable()
        {
            base.Enable();
            _lastDamageTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (_lastDamageTime + _damageDelay < Time.time)
            {
                _lastDamageTime = Time.time;
               // 이거 오류 나서 주석 처리해놓을께 레벨없이 이거 수정 부탁(2024-02-19(수)/최영환)
               // entity.GetEntityComponent<EntityHealth>().ApplyDamage(_damage[level <= 3 ? level - 1 : 2], default, null, attackType: EAttackType.Fire);
            }
        }

        public override void Disable()
        {
            base.Disable();
        }

        public void OnTimeOut()
        {

        }
    }
}
