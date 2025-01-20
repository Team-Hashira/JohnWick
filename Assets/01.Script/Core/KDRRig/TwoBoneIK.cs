using TMPro;
using UnityEngine;

namespace Hashira.Rigging
{
    public class TwoBoneIK : MonoBehaviour
    {
        [Header("Bones")]
        [SerializeField] private Bone _root;
        [SerializeField] private Bone _mid;
        [SerializeField] private Bone _tip;
        [Header("Target")]
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _hint;

        [ContextMenu("Setting")]
        private void Setting()
        {
            _mid = _root.nextBone;
            _tip = _mid.nextBone;

            if (_target == null)
            {
                GameObject gameObject = new GameObject("Target");
                gameObject.transform.SetParent(transform);
                _target = gameObject.transform;
                _target.position = _tip.transform.position;
            }
            if (_hint == null)
            {
                GameObject gameObject = new GameObject("Hint");
                gameObject.transform.SetParent(transform);
                _hint = gameObject.transform;
                _hint.position = _mid.transform.position;
            }
        }

        private void Update()
        {
            Vector3 targetPos = _target.position;
            float distance = Vector3.Distance(_root.transform.position, targetPos);
            if (distance > _root.length + _mid.length)
            {
                //_root.transform.rotation = Quaternion.FromToRotation(_mid.transform.position - _root.transform.position, targetPos - _root.transform.position);
                //_mid.transform.rotation = Quaternion.FromToRotation(_tip.transform.position - _mid.transform.position, targetPos - _mid.transform.position);
            }

            Vector3 targetDir = targetPos - _root.transform.position;
            float targetSqrDis = targetDir.sqrMagnitude;
            float cosAngle = (targetSqrDis + Mathf.Pow(_root.length, 2) - Mathf.Pow(_mid.length, 2)) / (2 * _root.length * targetDir.magnitude);

            cosAngle = Mathf.Clamp(cosAngle, -1, 1);
            Debug.Log(cosAngle);

            float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
            Debug.Log(angle);
            _root.transform.rotation = Quaternion.Euler(0, 0, angle);
            _mid.transform.rotation = Quaternion.FromToRotation(_tip.transform.localPosition, targetPos - _mid.transform.position);
        }
    }
}
