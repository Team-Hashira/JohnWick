using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class RegenerationModule : Module, IProjectileModifier
    {
        private EntityHealth _health;

        private int _shield = 0; 

        public override void Equip(Player player)
        {
            base.Equip(player);
            _health = player.GetEntityComponent<EntityHealth>();
            _player.Attacker.AddProjectileModifiers(this);
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
        }

        public void OnProjectileCreate(Projectile projectile)
        {
        }

        public void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
        }

        public void OnProjectileUpdate()
        {
            if (CooldownUtillity.CheckCooldown("Regeneration", 20f, true))
            {
                _health.ApplyRecovery((int)(_health.MaxHealth * 0.05f));
                if (_health.Health >= _health.MaxHealth)
                {
                    _shield = (int)(_health.MaxHealth * 0.1f);
                    _shield = Mathf.Clamp(_shield, 0, (int)(_health.MaxHealth * 0.1f));
                    _health.Shield += _shield;
                }
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            _player.Attacker.RemoveProjectileModifiers(this);
        }
    }
}
