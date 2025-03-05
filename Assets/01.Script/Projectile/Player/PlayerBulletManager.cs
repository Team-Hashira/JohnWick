using System;
using System.Collections.Generic;

namespace Hashira.Projectiles.Player
{
    public class PlayerBulletManager : MonoSingleton<PlayerBulletManager>
    {
        protected List<ProjectileModifier> _projectileModifiers = new List<ProjectileModifier>();
        private Attacker _playerAttacker;

        public int HandleOnModifier { get; private set; }

        public List<Projectile> projectileList;

        private void Start()
        {
            projectileList = new List<Projectile>();
            _playerAttacker = GameManager.Instance.Player.GetComponent<Attacker>();
        }

        public List<ProjectileModifier> GetModifierList => _projectileModifiers;

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
