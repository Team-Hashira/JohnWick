using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.SiegeEnemy
{
    public class SiegeEnemyVigilantState : EntityState
    {
        private SiegeEnemy _siegeEnemy;

        private EntityEmoji _entityEmoji;
        private EntityRenderer _entityRenderer;

        private float _vigilantStartTime;
        private float _vigilantTime = 3.5f;

        public SiegeEnemyVigilantState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _siegeEnemy = entity as SiegeEnemy;

            _entityEmoji = entity.GetEntityComponent<EntityEmoji>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _vigilantStartTime = Time.time;
            Vector3 soundPos = _entityStateMachine.GetShareVariable<Vector3>("SoundPosition");
            _entityRenderer.LookTarget(soundPos);
            _entityEmoji?.ShowEmoji(EEmotion.Question, 3f);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_vigilantStartTime + _vigilantTime < Time.time)
            {
                _entityStateMachine.ChangeState("Recon");
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
