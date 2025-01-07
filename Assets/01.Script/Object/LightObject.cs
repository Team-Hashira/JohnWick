using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Hashira.Object
{
    public class LightObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private Light2D _light;
        [SerializeField] private EffectPoolType _breakEffect;
        
        public EEntityPartType ApplyDamage(int value, Collider2D collider2)
        {
            _light.enabled = false;
            gameObject.Pop(_breakEffect, transform.position, Quaternion.identity);

            return EEntityPartType.Body;
        }
    }
}
