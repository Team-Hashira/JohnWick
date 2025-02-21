using Hashira.EffectSystem;
using System;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityEffector : MonoBehaviour, IEntityComponent
    {
        public Entity Entity { get; private set; }
        public Action<Effect> EffectAddedEvent;
        public Action<Effect> EffectRemovedEvent;

        public void Initialize(Entity entity)
        {
			Entity = entity;
		}
    }
}
