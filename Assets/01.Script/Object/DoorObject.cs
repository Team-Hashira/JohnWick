using Hashira.Entities;
using UnityEngine;

namespace Hashira
{
    public class DoorObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private ParticleSystem _breakParticle;

        public EEntityPartType ApplyDamage(int value, Collider2D collider2)
        {
            Instantiate(_breakParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);

            return EEntityPartType.Body;
        }
    }
}
