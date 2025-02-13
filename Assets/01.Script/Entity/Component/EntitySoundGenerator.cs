using Hashira.Core.EventSystem;
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

		public void SoundGenerate(float loudness)
		{
			var evt = SoundEvents.SoundGeneratedEvent;
			evt.originPosition = transform.position;
			evt.loudness = loudness;
			SoundEventChannel.RaiseEvent(evt);
		}
    }
}
