using Hashira.Combat;
using UnityEngine;

namespace Hashira
{
    public class Parallax : MonoBehaviour
    {
        //[SerializeField] private Vector2 _weight;

        [SerializeField] private float _depth;
        [SerializeField] private float _moveDistance;
        
        private Transform _followTarget;

        private Vector3 _originPosition;
        private Vector3 _offset;

        private void Start()
        {
            _followTarget = GameManager.Instance.Player.transform;
            _originPosition = transform.position;
            _offset = Vector3.zero;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(_originPosition, Camera.main.transform.position, _depth) + _offset;

            if (Mathf.Abs(transform.position.x - _followTarget.transform.position.x) > _moveDistance)
            {
                _offset += Vector3.right * _moveDistance * Mathf.Sign(_followTarget.transform.position.x - transform.position.x);
            }
        }
    }
}
