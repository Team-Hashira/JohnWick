using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.LatestFSM;
using UnityEngine;

namespace Hashira.Enemies
{
    public abstract class ListeningSoundState : EntityState
    {
        private GameEventChannelSO _soundEventChannel;
        protected string _targetState = "Chase";
        public ListeningSoundState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _soundEventChannel = (entity as Enemy)?.SoundEventChannel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _soundEventChannel.AddListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
        }

        private void HandleOnSoundGenerated(NearbySoundPointEvent evt)
        {
            _entityStateMachine.SetShareVariable("TargetNode", evt.node);
            _entityStateMachine.SetShareVariable("Loudness", evt.loudness);
            _entityStateMachine.ChangeState(_targetState);
        }

        public override void OnExit()
        {
            _soundEventChannel.RemoveListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
            base.OnExit();
        }
    }
}
