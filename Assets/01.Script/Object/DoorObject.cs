using Crogen.CrogenPooling;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Object
{
    public class DoorObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private EffectPoolType _breakParticle;

        public EEntityPartType ApplyDamage(int value, RaycastHit2D raycastHit, Transform attackerTrm)
        {
            gameObject.Pop(_breakParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);

            return EEntityPartType.Body;
        }
    }
}
