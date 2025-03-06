using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles
{
    //Todo
    public class RubberProjectileModifier : ProjectileModifier
    {
        private Dictionary<Projectile, int> _bounceCountDict;

        public override void OnEquip(Attacker attacker)
        {
            base.OnEquip(attacker);
            _bounceCountDict = new Dictionary<Projectile, int>();
        }

        public override void OnProjectileCreate(Projectile projectile)
        {
            base.OnProjectileCreate(projectile);
            projectile.CanDieSelf = false;
            if (_bounceCountDict.TryGetValue(projectile, out _))
            {
                _bounceCountDict[projectile] = 0;
            }
            else
                _bounceCountDict.Add(projectile, 0);
        }

        public override void OnProjectileHit(Projectile projectile, HitInfo hitInfo)
        {
            base.OnProjectileHit(projectile, hitInfo);
            if (_bounceCountDict[projectile] <= 3)
            {
                if (_bounceCountDict[projectile] == 2)
                    projectile.CanDieSelf = true;
                _bounceCountDict[projectile]++;
                Vector2 reflect = Vector2.Reflect(projectile.transform.right, hitInfo.raycastHit.normal);
                reflect.Normalize();
                projectile.Redirection(reflect);
                projectile.transform.position += (Vector3)reflect * projectile.Speed * 2 * Time.deltaTime;
                Debug.Log(_bounceCountDict[projectile]);
            }
        }

        public override void OnUnEquip()
        {
            base.OnUnEquip();
        }
    }
}
