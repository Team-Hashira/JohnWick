using Doryu.StatSystem;
using System;
using UnityEngine;

public enum EPlayerState
{
    Idle,
    Walk,
    Sprint,
    Crouch,
    Air
}

public class Player : Entity
{
    [field: SerializeField] public InputReaderSO InputReader { get; private set; }
    [field: SerializeField] public Transform VisualTrm { get; private set; }

    [SerializeField] private Gun _gun;

    protected StateMachine _stateMachine;

    protected RenderCompo _renderCompo;
    protected StatCompo _statCompo;

    protected StatElement _damageStat;

    protected override void Awake()
    {
        base.Awake();

        _stateMachine = new StateMachine(this);

        InputReader.OnAttackEvent += HandleAttackEvent;
    }

    protected override void ComponentInit()
    {
        base.ComponentInit();

        _statCompo = GetEntityComponent<StatCompo>();
        _renderCompo = GetEntityComponent<RenderCompo>();
        _damageStat = _statCompo.GetElement("AttackPower");
    }

    private void HandleAttackEvent()
    {
        _gun.Fire(_damageStat.IntValue);
        CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
    }

    protected override void Update()
    {
        base.Update();

        _stateMachine.MachineUpdate();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputReader.MousePosition);
        mousePos.z = 0;
        _gun.LookTarget(mousePos);

        _renderCompo.LookTarget(mousePos);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InputReader.OnAttackEvent -= HandleAttackEvent;
    }
}
