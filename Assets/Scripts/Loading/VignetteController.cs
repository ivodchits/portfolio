using System;
using UnityEngine;

namespace Loading
{
    public class VignetteController : MonoBehaviour
    {
        [SerializeField] GameObject _gameObject;
        [SerializeField] Material _vignetteMaterial;
        [SerializeField] float _duration = 0.4f;
        [SerializeField] float _pauseDuration = 0.8f;

        void Awake()
        {
            _vignetteMaterial.SetFloat(_valueId, 0);
            _vignetteMaterial.SetVector(_positionId, Vector2.zero);
        }

        public void Animate(Action callback, bool shrink, Vector2 position, float pauseValue = 0f)
        {
            _targetValue = shrink ? 0 : 1;
            _initialValue = shrink ? 1 : 0;
            _targetPosition = shrink ? position : Vector2.zero;
            _initialPosition = shrink ? Vector2.zero : position;
            _pauseValue = pauseValue;
            _callback = callback;
            _animating = true;
            _pauseTimer = 0;
            _timer = 0;
            _vignetteMaterial.SetFloat(_valueId, _initialValue);
            _vignetteMaterial.SetVector(_positionId, _initialPosition);
            _gameObject.SetActive(_animating);
        }

        void Update()
        {
            if (!_animating)
                return;

            var hasPause = _pauseValue > Epsilon && _pauseTimer < _pauseDuration;
            var target = hasPause ? _pauseValue : _targetValue;
            var duration = hasPause ? _duration - _pauseValue * _duration : _duration;
            var value = Mathf.Lerp(_initialValue, target, _timer / duration);
            var position = Vector2.Lerp(_initialPosition, _targetPosition, _timer / (_duration - _pauseValue * _duration));
            if (Mathf.Abs(value - target) < Epsilon)
            {
                if (hasPause)
                {
                    _pauseTimer += Time.deltaTime;
                    if (_pauseTimer < Epsilon)
                    {
                        _vignetteMaterial.SetFloat(_valueId, _pauseValue);
                        _vignetteMaterial.SetVector(_positionId, _targetPosition);
                    }
                    return;
                }
                _animating = false;
                _vignetteMaterial.SetFloat(_valueId, _targetValue);
                _vignetteMaterial.SetVector(_positionId, _targetPosition);
                _gameObject.SetActive(_targetValue < 1);
                _callback?.Invoke();
                return;
            }

            _timer += Time.deltaTime;

            _vignetteMaterial.SetFloat(_valueId, value);
            _vignetteMaterial.SetVector(_positionId, position);
        }

        readonly int _valueId = Shader.PropertyToID("_Value");
        readonly int _positionId = Shader.PropertyToID("_Position");

        Action _callback;
        float _timer;
        float _pauseTimer;
        float _pauseValue;
        float _targetValue;
        float _initialValue;
        Vector2 _targetPosition;
        Vector2 _initialPosition;
        bool _animating;

        const float Epsilon = 0.01f;
    }
}
