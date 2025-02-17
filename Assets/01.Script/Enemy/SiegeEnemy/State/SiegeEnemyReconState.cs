using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.SiegeEnemy
{
    public class SiegeEnemyReconState : EnemyListeningSoundState
    {
        private SiegeEnemy _siegeEnemy;

        private EntityEmoji _entityEmoji;
        private EntityRenderer _entityRenderer;

        private float _flipTimer = 0f;
        private float _flipDelay = 2f;

        public SiegeEnemyReconState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _targetState = "Vigilant";

            _siegeEnemy = entity as SiegeEnemy;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _flipTimer = 0;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _flipTimer += Time.deltaTime;
            if (_flipTimer > _flipDelay)
            {
                _entityRenderer.Flip();
                _flipTimer = 0f;
            }
            Player player = _siegeEnemy.DetectPlayer();
            if (player != null)
            {
                _entityEmoji?.ShowEmoji(EEmotion.Surprise, 1f);
                _entityStateMachine.SetShareVariable("Target", player);
                _entityStateMachine.ChangeState("Attack");
            }
        }
    }
}
