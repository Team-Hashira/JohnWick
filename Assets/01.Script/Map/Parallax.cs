using UnityEngine;

namespace Hashira
{
    public class Parallax : MonoBehaviour
    {
        //[SerializeField] private Vector2 _weight;

        [SerializeField] private float _depth;

        private Vector3 _originPosition;

        private void Start()
        {
            _originPosition = transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(_originPosition, Camera.main.transform.position, _depth);
        }
    }
}
