using Crogen.CrogenPooling;
using Hashira.Players;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Items.Modules
{
    public class BlastBulletModule : Module, IProjectileModifier
    {
        private bool _isCanBlastBullet = true;
        float _delay = 8f;
        public int doubleAttackDamage = 100;
        public Projectile _projectile;

        public override void Equip(Player player)
        {
            base.Equip(player);
            player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
            player.Attacker.AddProjectileModifiers(this);
        }

        private void HandleProjectileCreateEvent()
        {
            _player.Attacker.OnProjectileCreateEvent -= HandleProjectileCreateEvent;
            _player.Attacker.RemoveProjectileModifiers(this);
            CooldownUtillity.StartCooldown("BlastBullet");
            _isCanBlastBullet = false;
        }

        public override void ItemUpdate()
        {
            base.ItemUpdate();
            if (CooldownUtillity.CheckCooldown("BlastBullet", _delay) && _isCanBlastBullet == false)
            {
                _isCanBlastBullet = true;
                _player.Attacker.OnProjectileCreateEvent += HandleProjectileCreateEvent;
                _player.Attacker.AddProjectileModifiers(this);
            }
        }

        public override void UnEquip()
        {
            base.UnEquip();
            if (CooldownUtillity.CheckCooldown("AimingBullet", _delay))
                HandleProjectileCreateEvent();
        }

        public void OnProjectileCreate(Projectile projectile)
        {
            for (int i = -1; i <= 1; i++)
            {
                var blastBulletModuleSplinter = PopCore.Pop(ProjectilePoolType.BlastBulletModuleSplinter, _projectile.transform.position + Vector3.up * 0.25f, Quaternion.Euler(0, 0, Random.Range(0, 360f))) as BlastBulletModuleSplinter;
                Vector2 dir = new Vector2(i * 0.55f, 1).normalized * 300;
                dir += Random.insideUnitCircle.normalized * 10f;
                blastBulletModuleSplinter.Init(dir, doubleAttackDamage);
            }
        }

        public void OnProjectileUpdate()
        {

        }

        public void OnProjectileHit(RaycastHit2D hit, IDamageable damageable)
        {
            doubleAttackDamage = (int)(_projectile.Damage * 0.3f);
        }
    }
}
