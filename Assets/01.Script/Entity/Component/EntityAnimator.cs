using Hashira.Players;
using UnityEngine;

namespace Hashira.Entities.Components
{
    [RequireComponent(typeof(Animator))]
    public class EntityAnimator : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        private readonly static int _MoveDirAnimationHash = Animator.StringToHash("MoveDir");

        [field: SerializeField] public Animator Animator { get; private set; }

        private Entity _entity;
        private EntityMover _mover;
        private EntityRenderer _renderer;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void AfterInit()
        {
            _mover = _entity.GetEntityComponent<PlayerMover>();
            _renderer = _entity.GetEntityComponent<EntityRenderer>();
        }

        private void Update()
        {
            if (_mover != null && _renderer != null)
            {
                float moveDir = Mathf.Sign(_mover.Velocity.x) * _renderer.FacingDirection;
                SetParam(_MoveDirAnimationHash, moveDir);
            }
        }

        #region Param Func
        public void SetParam(int hash, float value) => Animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => Animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => Animator.SetBool(hash, value);
        public void SetParam(int hash) => Animator.SetTrigger(hash);
        #endregion
    }
}
