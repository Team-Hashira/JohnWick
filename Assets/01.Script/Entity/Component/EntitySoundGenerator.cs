using Crogen.CrogenPooling;
using Hashira.Core.EventSystem;
using Hashira.VFX;
using UnityEngine;

namespace Hashira.Entities.Components
{
	public class EntitySoundGenerator : MonoBehaviour, IEntityComponent
    {
		[field: SerializeField]
		public GameEventChannelSO SoundEventChannel { get; private set; }

		protected Entity _entity;

		public void Initialize(Entity entity)
		{
			_entity = entity;
		}

		public void SoundGenerate(float loudness, Vector3 offset = default)
		{
			var evt = SoundEvents.SoundGeneratedEvent;
			evt.originPosition = transform.position + offset;
			evt.loudness = loudness;
			SoundEventChannel.RaiseEvent(evt);

			SoundEffect soudnEffect = gameObject.Pop(
				EffectPoolType.SoundEffect, 
				evt.originPosition, 
				Quaternion.identity) as SoundEffect;
			soudnEffect.Init(loudness/2);
		}
    }
}
