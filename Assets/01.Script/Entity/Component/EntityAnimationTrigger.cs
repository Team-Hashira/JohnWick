using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    [Flags]
    public enum EAnimationTrigger
    {
        End,
        Attack
    }

    public class EntityAnimationTrigger : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;

        public event Action<EAnimationTrigger> OnAnimationTriggeredEvent;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void TriggerAction(EAnimationTrigger trigger)
        {
            Debug.Log(trigger.ToString());
            OnAnimationTriggeredEvent?.Invoke(trigger);
        }
    }
}
