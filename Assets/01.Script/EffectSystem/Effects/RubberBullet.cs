using Hashira.Core.StatSystem;
using Hashira.Projectiles;
using Hashira.Projectiles.Player;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class RubberBullet : Effect
    {
        public float Duration { get; set; } = 5;
        public float Time { get; set; }
        public float Amount { get; set; }

        public override int MaxActiveCount => -1;

        private RubberProjectileModifier _rubberProjectileModifier;

        public override void Enable()
        {
            base.Enable();
            _rubberProjectileModifier = new RubberProjectileModifier();
            PlayerBulletManager.Instance.EquipBulletModifier(_rubberProjectileModifier);
        }

        public override void Disable()
        {
            base.Disable();
            PlayerBulletManager.Instance.UnEquipBulletModifier(_rubberProjectileModifier);
        }

        public void OnTimeOut()
        {
        }
    }
}
