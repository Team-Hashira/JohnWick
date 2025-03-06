using Hashira.Core;
using Hashira.Players;
using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Hashira.UI
{
    public class StaminaUI : MonoBehaviour
    {
        private readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [SerializeField] private SpriteRenderer _visualRenderer;
        [SerializeField] private SpriteRenderer _backgroundRenderer;

        private Color _visualColor;
        private Color _backgroundColor;

        private Player _player;
        private Material _material;

        private float _displayTime = 2f;
        private float _currentDisableDelay = 0;
        private float _disableDuration = 0.5f;

        private void Awake()
        {
            _player = PlayerManager.Instance.Player;

            _visualColor = _visualRenderer.color;
            _backgroundColor = _backgroundRenderer.color;

            _player.OnStaminaChangedEvent += HandleStaminaChangedEvent;
        }

        private void Start()
        {
            _material = _visualRenderer.material;
        }

        private void HandleStaminaChangedEvent(float prevValue, float newValue)
        {
            if (prevValue > newValue)
            {
                _currentDisableDelay = _displayTime;
                _visualRenderer.color = _visualColor;
                _backgroundRenderer.color = _backgroundColor;
            }
            else if (newValue == _player.MaxStamina)
            {
                _currentDisableDelay = _disableDuration;
                _visualRenderer.color = _visualColor;
                _backgroundRenderer.color = _backgroundColor;
            }
            _material.SetFloat(_FillAmountShaderHash, newValue / _player.MaxStamina);
        }

        private void Update()
        {
            if (_currentDisableDelay > 0)
            {
                _currentDisableDelay -= Time.deltaTime;

                if (_currentDisableDelay < 0) _currentDisableDelay = 0;

                if (_currentDisableDelay <= _disableDuration)
                {
                    float alpha = _currentDisableDelay / _disableDuration;
                    SetAlpha(alpha);
                }
            }
        }

        private void SetAlpha(float alpha)
        {
            Color visualColor = _visualRenderer.color;
            Color backgroundColor = _backgroundRenderer.color;
            visualColor.a = alpha;
            backgroundColor.a = alpha;
            _visualRenderer.color = visualColor;
            _backgroundRenderer.color = backgroundColor;
        }

        private void OnDestroy()
        {
            _player.OnStaminaChangedEvent -= HandleStaminaChangedEvent;
        }
    }
}
