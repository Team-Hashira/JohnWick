using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities
{
    public enum EEntityPartType
    {
        Head,
        Body,
        Legs
    }

    public class EntityPartCollider : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Collider2D _head, _body, _legs;
        private Dictionary<EEntityPartType, Collider2D> _partDictionary;

        private Entity _entity;

        public event Action<EEntityPartType> OnPartCollisionHitEvent;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _partDictionary = new Dictionary<EEntityPartType, Collider2D>()
        {
            { EEntityPartType.Head, _head },
            { EEntityPartType.Body, _body },
            { EEntityPartType.Legs, _legs }
        };
        }

        public EEntityPartType Hit(Collider2D collider)
        {
            EEntityPartType parts = _partDictionary.Keys.FirstOrDefault(x => _partDictionary[x] == collider);
            OnPartCollisionHitEvent?.Invoke(parts);
            return parts;
        }
    }
}