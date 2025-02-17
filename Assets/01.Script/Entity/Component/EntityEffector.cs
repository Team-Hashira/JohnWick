using UnityEngine;

namespace Hashira.Entities
{
    public class EntityEffector : MonoBehaviour, IEntityComponent
    {
        public Entity Entity { get; private set; }

        public void Initialize(Entity entity)
        {
			Entity = entity;
		}
    }
}
