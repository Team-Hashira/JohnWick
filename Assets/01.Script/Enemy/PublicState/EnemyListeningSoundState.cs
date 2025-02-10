using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies
{
    public abstract class EnemyListeningSoundState : EntityState
    {
        private GameEventChannelSO _soundEventChannel;
        private EntityStat _entityStat;

        private StatElement _hearingElement;

        protected string _targetState = "Chase";

        public EnemyListeningSoundState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _soundEventChannel = (entity as Enemy)?.SoundEventChannel;
            _entityStat = entity.GetEntityComponent<EntityStat>();

            _hearingElement = _entityStat.StatDictionary["Hearing"];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _soundEventChannel.AddListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
        }

        private void HandleOnSoundGenerated(NearbySoundPointEvent evt)
        {
            float distance = Vector2.Distance(_entity.transform.position, evt.originPosition);
            float audibility = distance - evt.loudness * 0.5f; // 임시 계산식

            if (_hearingElement.Value > audibility) // 들을 수 있는거리.
            {
                _entityStateMachine.SetShareVariable("TargetNode", evt.node);
                _entityStateMachine.SetShareVariable("Loudness", evt.loudness);
                _entityStateMachine.ChangeState(_targetState);
            }
        }

        public override void OnExit()
        {
            _soundEventChannel.RemoveListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
            base.OnExit();
        }
    }
}
