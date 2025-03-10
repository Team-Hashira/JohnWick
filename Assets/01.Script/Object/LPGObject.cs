using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Object
{
    public class LPGObject : MonoBehaviour, IDamageable
    {
        public bool IsEvasion { get; set; }

        [SerializeField] private int _explosionDamage;
        [SerializeField] private EffectPoolType _explosionEffectPoolType;

        private CircleDamageCaster2D _circleDamageCaster;

        private void Awake()
        {
            _circleDamageCaster = GetComponentInChildren<CircleDamageCaster2D>();
        }

        public EEntityPartType ApplyDamage(int value, RaycastHit2D raycastHit, Transform attackerTrm, Vector2 knockback = default, EAttackType attackType = EAttackType.Default)
        {
            var explosionEffect = PopCore.Pop(_explosionEffectPoolType, transform.position, Quaternion.identity) as SimplePoolingObject;
            var originScale = explosionEffect.gameObject.transform.localScale;

            explosionEffect.gameObject.transform.localScale = Vector3.one * _circleDamageCaster.radius;

            _circleDamageCaster.CastDamage(_explosionDamage, Vector3.zero, knockback, EAttackType.Fire);

            Destroy(gameObject);
            return EEntityPartType.Body;
        }
    }
}
