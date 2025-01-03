using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        private readonly static int _BlinkShanderkHash = Shader.PropertyToID("_Blink");

        private Entity _entity;

        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [SerializeField] private List<SpriteRenderer> _spriteRendererList;
        [SerializeField] private bool _isFlip;

        public float FacingDirection { get; private set; }

        private List<Tween> _blinkTweenList;

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _blinkTweenList = new List<Tween>();
            FacingDirection = 1;
        }

        public void Blink(float duration, Ease ease = Ease.Linear)
        {
            foreach (Tween tween in _blinkTweenList)
            {
                tween.Kill();
            }
            _blinkTweenList.Clear();

            _spriteRendererList.ForEach(renderer =>
            {
                renderer.material.SetFloat(_BlinkShanderkHash, 1);
                _blinkTweenList.Add(renderer.material.DOFloat(0, _BlinkShanderkHash, duration).SetEase(ease));
            });
        }

        public void LookTarget(Vector3 pos)
        {
            if (Mathf.Abs(FacingDirection + Mathf.Sign(pos.x - _entity.transform.position.x)) < 1)
                Flip();
        }

        private void Flip()
        {
            FacingDirection *= -1;
            VisualTrm.localEulerAngles = new Vector3(0, ((FacingDirection > 0) ^ _isFlip) ? 0 : 180, 0);
        }
    }
}
