using UnityEngine;

namespace Hashira
{
    public class StagePice : MonoBehaviour
    {
        [SerializeField] private Transform _inPoint, _outPoint;
        public Vector3 InPoint => _inPoint.position;
        public Vector3 OutPoint => _outPoint.position;

        protected StageGenerator _stageGenerator;

        public virtual void Init(Vector3 startPosition)
        {
            transform.position = (Vector2)startPosition - (Vector2)_inPoint.localPosition * (Vector2)transform.localScale;
        }
    }
}