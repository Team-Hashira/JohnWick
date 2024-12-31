using Doryu.StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerState
{
    Idle,
    Walk,
    Air
}

public class Player : Entity
{
    [field: SerializeField] public InputReaderSO InputReader { get; private set; }


    protected StateMachine _stateMachine;

    protected override void Awake()
    {
        base.Awake();

        _stateMachine = new StateMachine(this);
    }

    protected override void Update()
    {
        base.Update();

        _stateMachine.MachineUpdate();
    }
}
