using System;
using System.Collections.Generic;

namespace Hashira.Projectiles.Player
{
    public class PlayerBulletManager : MonoSingleton<PlayerBulletManager>
    {
        protected List<ProjectileModifier> _projectileModifiers = new List<ProjectileModifier>();
        private Attacker _playerAttacker;

        private void Start()
        {
            _playerAttacker = GameManager.Instance.Player.GetComponent<Attacker>();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _projectileModifiers.Count; i++)
                _projectileModifiers[i].OnProjectileUpdate();
        }

        public void EquipBulletModifier(ProjectileModifier projectileModifier)
        {
            _projectileModifiers.Add(projectileModifier);
            projectileModifier.OnEquip(_playerAttacker);
        }

        public void UnEquipBulletModifier(ProjectileModifier projectileModifier)
        {
            _projectileModifiers.Remove(projectileModifier);
            projectileModifier.OnUnEquip();
        }
    }
}
