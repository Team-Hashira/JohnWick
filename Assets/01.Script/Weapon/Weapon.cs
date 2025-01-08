using System;
using UnityEngine;

namespace Hashira.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected Transform _visualTrm;
        protected float _facing;

        public void LookTarget(Vector3 targetPos)
        {
            _facing = MathF.Sign(targetPos.x - transform.position.x);
            Vector3 dir = targetPos - transform.position;
            transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg * _facing);
            //_visualTrm.localScale = new Vector3(1, _facing, 1);
            //Vector2 center = _meleeAttackCenter;
            //center.y *= _facing;
            //_boxDamageCaster2D.center = center;
        }

        public abstract void MeleeAttack(int damage);
        public abstract bool MainAttack(int damage);
    }
}
