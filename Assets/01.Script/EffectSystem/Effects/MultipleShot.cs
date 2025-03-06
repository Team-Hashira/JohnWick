using Hashira.Core;
using Hashira.Core.StatSystem;
using Hashira.Projectiles;
using Hashira.Projectiles.Player;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class MultipleShot : Effect, ICoolTimeEffect
    {
        public float Duration { get; set; } = 5;
        public float Time { get; set; }

        public override int MaxActiveCount => 4;

        private ShootCountProjectileModifier _shootCountProjectileModifier;

        public override void Enable()
        {
            base.Enable();
            _shootCountProjectileModifier = new ShootCountProjectileModifier();
            _shootCountProjectileModifier.Init(1);
            PlayerBulletManager.Instance.EquipBulletModifier(_shootCountProjectileModifier);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Disable()
        {
            base.Disable();
            PlayerBulletManager.Instance.UnEquipBulletModifier(_shootCountProjectileModifier);
        }

        public void OnTimeOut()
        {
        }
    }
}
