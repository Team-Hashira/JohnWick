using Hashira.Entities;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Hashira
{
    public class LightObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private Light2D _light;
        [SerializeField] private ParticleSystem _breakEffct;

        public EEntityPartType ApplyDamage(int value, Collider2D collider2)
        {
            _light.enabled = false;
            Instantiate(_breakEffct, transform.position, Quaternion.identity);

            return EEntityPartType.Body;
        }
    }
}
