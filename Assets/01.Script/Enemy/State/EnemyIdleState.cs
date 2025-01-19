using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using System;
using UnityEngine;

namespace Hashira.Enemies
{
    public class EnemyIdleState : EntityState
    {
        private float _idleStartTime;
        private float _waitDelay = 2f;
        private GameEventChannelSO _soundEventChannel;
        public EnemyIdleState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _soundEventChannel = (entity as Enemy).SoundEventChannel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _idleStartTime = Time.time;
            _soundEventChannel.AddListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
        }

        private void HandleOnSoundGenerated(NearbySoundPointEvent evt)
        {
            _entityStateMachine.SetShareVariable("TargetNode", evt.node);
            _entityStateMachine.SetShareVariable("Loudness", evt.loudness);
            _entityStateMachine.ChangeState("Chase");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_idleStartTime + _waitDelay < Time.time)
            {
                _entityStateMachine.ChangeState("Patrol");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
