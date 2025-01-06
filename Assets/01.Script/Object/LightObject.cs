using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hashira
{
    public class LightObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private Light2D _light;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private EffectPoolType _breakEffct;

        public EEntityPartType ApplyDamage(int value, Collider2D collider2)
        {
            _light.enabled = false;
            _collider.enabled = false;
            gameObject.Pop(_breakEffct, transform.position, Quaternion.identity);

            return EEntityPartType.Body;
        }
    }
}
