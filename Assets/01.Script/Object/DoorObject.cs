using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;

namespace Hashira
{
    public class DoorObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private EffectPoolType _breakParticle;

        public EEntityPartType ApplyDamage(int value, Collider2D collider2)
        {
            gameObject.Pop(_breakParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);

            return EEntityPartType.Body;
        }
    }
}
