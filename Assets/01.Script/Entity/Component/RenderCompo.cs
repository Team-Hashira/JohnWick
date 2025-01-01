using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class RenderCompo : MonoBehaviour, IEntityComponent
{
    private readonly static int _BlinShanderkHash = Shader.PropertyToID("_Blink");

    private Entity _entity;

    [field:SerializeField] public Transform VisualTrm {  get; private set; }
    [SerializeField] private List<SpriteRenderer> _spriteRenderers;
    [SerializeField] private bool _isFlip;

    public float Facing { get; private set; }

    private List<Tween> _blinkTweenList;

    public void Initialize(Entity entity)
    {
        _entity = entity;

        _blinkTweenList = new List<Tween>();
        Facing = 1;
    }

    public void Blink(float duration, Ease ease = Ease.Linear)
    {
        foreach (Tween tween in _blinkTweenList)
        {
            tween.Kill();
        }
        _blinkTweenList.Clear();

        _spriteRenderers.ForEach(renderer =>
        {
            renderer.material.SetFloat(_BlinShanderkHash, 1);
            _blinkTweenList.Add(renderer.material.DOFloat(0, _BlinShanderkHash, duration).SetEase(ease));
        });
    }

    public void LookTarget(Vector3 pos)
    {
        if (Mathf.Abs(Facing + Mathf.Sign(pos.x - _entity.transform.position.x)) < 1)
            Flip();
    }

    private void Flip()
    {
        Facing *= -1;
        VisualTrm.localEulerAngles = new Vector3(0, ((Facing > 0) ^ _isFlip) ? 0 : 180, 0);
    }
}
