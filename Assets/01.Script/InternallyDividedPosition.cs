using UnityEngine;

namespace Hashira
{
    public class InternallyDividedPosition : MonoBehaviour
    {
        [SerializeField] private bool _isCreenStartPos;
        [SerializeField] private Transform _startPosition;
        [SerializeField] private bool _isCreenEndPos;
        [SerializeField] private Transform _endPosition;
        [Range(0, 1)]
        [SerializeField] private float _amount;

        private void Update()
        {
            Vector2 startPos;
            Vector2 endPos;

            if (_isCreenStartPos) startPos = Camera.main.ScreenToWorldPoint(_startPosition.position);
            else startPos = _startPosition.position;

            if (_isCreenEndPos) endPos = Camera.main.ScreenToWorldPoint(_endPosition.position);
            else endPos = _endPosition.position;

            transform.position = startPos * (1 - _amount) + endPos * _amount;
        }
    }
}