using Hashira.Entities;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntityWeapon : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IEntityDisposeComponent
    {
        public virtual void Initialize(Entity entity)
        {

        }

        public virtual void AfterInit()
        {

        }

        public virtual void Attack(int damage, bool isDown)
        { 

        }

        public virtual void Dispose()
        {

        }
    }
}
