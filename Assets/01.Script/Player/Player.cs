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

    [SerializeField] private Gun _gun;

    protected StateMachine _stateMachine;

    protected override void Awake()
    {
        base.Awake();

        _stateMachine = new StateMachine(this);

        InputReader.OnAttackEvent += HandleAttackEvent;
    }

    private void HandleAttackEvent()
    {
        _gun.Fire();
        CameraManager.Instance.ShakeCamera(8, 10, 0.15f);
    }

    protected override void Update()
    {
        base.Update();

        _stateMachine.MachineUpdate();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(InputReader.MousePosition);
        mousePos.z = 0;
        _gun.LookTarget(mousePos);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InputReader.OnAttackEvent -= HandleAttackEvent;
    }
}
