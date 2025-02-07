using Hashira.Entities;
using Hashira.FSM;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hashira.Players
{
    public class PlayerCrouchState : PlayerGroundState
    {
        private PlayerMover _playerMover;

        public PlayerCrouchState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _playerMover = entity.GetEntityComponent<PlayerMover>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityMover.StopImmediately();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void HandleJumpEvent()
        {
            _player.StartCoroutine(UnderJumpCoroutime());
        }

        private IEnumerator UnderJumpCoroutime()
        {
            _playerMover.UnderJump(true);
            yield return new WaitForSeconds(0.2f);
            _playerMover.UnderJump(false);
        }
    }
}
