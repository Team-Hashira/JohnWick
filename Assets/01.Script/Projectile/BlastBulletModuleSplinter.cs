using Crogen.CrogenPooling;
using UnityEngine;

namespace Hashira.Projectiles
{
    public class BlastBulletModuleSplinter : MonoBehaviour, IPoolingObject
    {
        private Rigidbody2D _rigidbody;
        private CircleDamageCaster2D _circleDamageCast;

        private int _damage;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _circleDamageCast = GetComponent<CircleDamageCaster2D>();
            _circleDamageCast.OnCasterSuccessEvent += HandleOnCastDamage;
        }

        private void HandleOnCastDamage(RaycastHit2D _)
        {
            this.Push();
        }

        public void Init(Vector2 dir, int damage)
        {
            _rigidbody.AddForce(dir, ForceMode2D.Impulse);
            _damage = damage;
        }

        private void Update()
        {
            transform.eulerAngles += new Vector3(0, 0, Time.deltaTime * 10);
            _circleDamageCast.CastDamage(_damage);
        }

        public void OnPop()
        {
        }

        public void OnPush()
        {
        }
    }
}
