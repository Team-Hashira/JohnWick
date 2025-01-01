using System;
using UnityEngine;

public class Enemy : Entity
{
    protected RenderCompo _renderCompo;
    protected HealthCompo _healthCompo;

    //Test
    [SerializeField] private Player _player;
    [SerializeField] private Transform _dieEffect;

    protected override void Awake()
    {
        base.Awake();

        GetEntityComponent<PartsColliderCompo>().OnPartsCollisionHitEvent += HandlePartsCollisionHitEvent;
        _healthCompo.OnDieEvent += HandleDieEvent;
    }

    private void HandleDieEvent()
    {
        Instantiate(_dieEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected override void ComponentInit()
    {
        base.ComponentInit();

        _renderCompo = GetEntityComponent<RenderCompo>();
        _healthCompo = GetEntityComponent<HealthCompo>();
    }

    protected override void Update()
    {
        base.Update();

        _renderCompo.LookTarget(_player.transform.position);
    }

    private void HandlePartsCollisionHitEvent(EEntityParts parts)
    {
        if (parts == EEntityParts.Head)
        {
            _renderCompo.Blink(0.2f, DG.Tweening.Ease.InCirc);
        }
    }
}
