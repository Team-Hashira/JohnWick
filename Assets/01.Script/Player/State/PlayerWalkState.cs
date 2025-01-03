using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerWalkState : PlayerGroundState
    {
        private StatElement _speedStat;

        public PlayerWalkState(Player owner, StateMachine stateMachine, string animationName) : base(owner, stateMachine, animationName)
        {
            _speedStat = owner.GetEntityComponent<EntityStat>().GetElement("Speed");
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (_owner.InputReader.XMovement != 0)
            {
                if (_owner.InputReader.IsSprint == false)
                {
                    float movement = _owner.InputReader.XMovement;
                    if (_speedStat != null)
                        movement *= _speedStat.Value;
                    _entityMover.SetMovement(movement);
                }
                else
                {
                    _stateMachine.ChangeState(EPlayerState.Sprint);
                }
            }
            else
            {
                _stateMachine.ChangeState(EPlayerState.Idle);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}