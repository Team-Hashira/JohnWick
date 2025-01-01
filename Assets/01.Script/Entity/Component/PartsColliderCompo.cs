using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EEntityParts
{
    Head,
    Body,
    Legs
}

public class PartsColliderCompo : MonoBehaviour, IEntityComponent
{
    [SerializeField] private Collider2D _head, _body, _legs;
    private Dictionary<EEntityParts, Collider2D> _partsDictionary;

    private Entity _entity;

    public event Action<EEntityParts> OnPartsCollisionHitEvent;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        _partsDictionary = new Dictionary<EEntityParts, Collider2D>()
        {
            { EEntityParts.Head, _head },
            { EEntityParts.Body, _body },
            { EEntityParts.Legs, _legs }
        };
    }

    public EEntityParts Hit(Collider2D collider)
    {
        EEntityParts parts = _partsDictionary.Keys.FirstOrDefault(x => _partsDictionary[x] == collider);
        OnPartsCollisionHitEvent?.Invoke(parts);
        return parts;
    }
}
