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
        //[SerializeField] private List<GameObject> _armObject;
        [SerializeField] private bool _onFlip;

        public List<SpriteRenderer> SpriteRendererList;
        public float FacingDirection { get; private set; }
        public Vector3 UsualFacingTarget { get; private set; }
        public bool isUsualFacing = true;

        private List<Tween> _blinkTweenList;

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _blinkTweenList = new List<Tween>();
            FacingDirection = 1;
            isUsualFacing = true;

            SetArmActive(true);
        }

        public void SetArmActive(bool active)
        {
            //_armObject.ForEach(gameObject => gameObject.SetActive(active));
        }

        public void Blink(float duration, Ease ease = Ease.Linear)
        {
            foreach (Tween tween in _blinkTweenList)
            {
                tween.Kill();
            }
            _blinkTweenList.Clear();

            SpriteRendererList.ForEach(renderer =>
            {
                renderer.material.SetFloat(_BlinkShanderkHash, 1);
                _blinkTweenList.Add(renderer.material.DOFloat(0, _BlinkShanderkHash, duration).SetEase(ease));
            });
        }

        private void Update()
        {
            if (isUsualFacing && UsualFacingTarget != Vector3.zero)
            {
                LookTarget(UsualFacingTarget);
            }
        }

        public void SetUsualFacingTarget(Vector3 pos)
        {
            UsualFacingTarget = pos;
        }

        public void LookTarget(Vector3 pos)
        {
            if (Mathf.Abs(FacingDirection + Mathf.Sign(pos.x - _entity.transform.position.x)) < 1)
                Flip();
        }

        public void Flip()
        {
            FacingDirection *= -1;
            VisualTrm.localEulerAngles = new Vector3(0, ((FacingDirection > 0) ^ _onFlip) ? 0 : 180, 0);
        }
    }
}
