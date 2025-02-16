using Crogen.CrogenPooling;
using Hashira.Combat;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class Bullet : Projectile, IParryingable
    {
        public bool IsParryingable { get; set; }
        public Transform Owner { get; set; }

        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, int penetration, Transform owner)
        {
            base.Init(whatIsTarget, direction, speed, damage, penetration, owner);
            Owner = owner;
            IsParryingable = true;
        }

        public void Parrying(LayerMask whatIsNewTargetLayer, Transform owner, bool isChargedParrying)
        {
            if (IsParryingable == false) return;

            Quaternion effectRotation = transform.rotation * Quaternion.Euler(0, 0, -90);
            gameObject.Pop(EffectPoolType.HitSparkleEffect, transform.position, effectRotation);

            if (isChargedParrying)
            {
                CameraManager.Instance.ShakeCamera(15, 11, 0.25f);
                _damage *= 10;
                _speed *= 10;
                gameObject.Pop(EffectPoolType.HitSparkleEffect, transform.position, effectRotation);
            }
            else
                CameraManager.Instance.ShakeCamera(4, 4, 0.15f);

            _whatIsTarget = whatIsNewTargetLayer;
            IsParryingable = false;
            Owner = owner;
            transform.localEulerAngles += new Vector3(0, 180, 0);
        }
    }
}