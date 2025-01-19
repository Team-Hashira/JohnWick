using Crogen.CrogenPooling;
using TMPro;
using UnityEngine;

namespace Hashira
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float _upSpeed;
        [SerializeField] private float _xWobbleSpeed;
        [SerializeField] private float _xWobblePower;
        [SerializeField] private SimplePoolingObject _simplePoolingObject;

        private TextMeshPro _damageText;
        private float _spawnTime;

        private void Awake()
        {
            _damageText = transform.GetComponentInChildren<TextMeshPro>();
        }

        public void Init(int damage, float spread = 0.3f)
        {
            transform.position += (Vector3)Random.insideUnitCircle * spread;
            _damageText.text = damage.ToString();
            _damageText.color = Color.white;
            _damageText.alpha = 1.0f;
        }
        public void Init(int damage, Color color, float spread = 0.3f)
        {
            transform.position += (Vector3)Random.insideUnitCircle * spread;
            _damageText.text = damage.ToString();
            _damageText.color = color;
            _damageText.alpha = 1.0f;
        }

        private void Update()
        {
            transform.position += Vector3.up * Time.deltaTime * _upSpeed;
            _damageText.transform.localPosition 
                = new Vector3(Mathf.Sin(_simplePoolingObject.CurLifetime * _xWobbleSpeed) * _xWobblePower, 0, 0);
            _damageText.alpha = 1 - _simplePoolingObject.CurLifetime / _simplePoolingObject.duration;
        }
    }
}
