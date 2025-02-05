using Crogen.CrogenPooling;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Object
{
    public class DoorObject : MonoBehaviour, IDamageable
    {
        [SerializeField] private EffectPoolType _breakParticle;
        [SerializeField] private List<Sprite> _spriteList;
        private SpriteRenderer _spriteRenderer;

        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; }

		private void Awake()
		{
			_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
			CurrentHealth = MaxHealth;
		}

		public EEntityPartType ApplyDamage(int value, RaycastHit2D raycastHit, Transform attackerTrm)
        {
			CurrentHealth -= value;

            var curPercent = CurrentHealth / (float)MaxHealth;
            if(_spriteList.Count > 0)
			    _spriteRenderer.sprite = _spriteList[(int)(curPercent * _spriteList.Count)];

			gameObject.Pop(_breakParticle, transform.position, Quaternion.identity);

            if(CurrentHealth <= 0)
                Destroy(gameObject);

            return EEntityPartType.Body;
        }
    }
}
