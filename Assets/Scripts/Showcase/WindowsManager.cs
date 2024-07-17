using System;
using System.Collections;
using UnityEngine;

namespace Showcase
{
    public class WindowsManager : MonoBehaviour
    {
        [SerializeField] Transform _playerTransform;
        [SerializeField] Window[] _windows;
        [SerializeField] float _distanceToOpen = 5;

        IEnumerator Start()
        {
            foreach (var window in _windows)
            {
                if (window.ShouldLoadVideo)
                {
                    window.LoadVideo();
                    yield return _waitForSecond;
                }
            }
        }

        void Update()
        {
            if (!_active)
            {
                _active |= Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
                return;
            }

            var minDistance = float.MaxValue;
            var window = default(Window);
            var foundWindow = false;
            for (int i = 0; i < _windows.Length; i++)
            {
                var currentWindow = _windows[i];
                var distance = Mathf.Abs(_playerTransform.position.x - currentWindow.Transform.position.x);
                if (distance < minDistance && distance < _distanceToOpen)
                {
                    minDistance = distance;
                    window = currentWindow;
                    foundWindow = true;
                }
            }

            if (window != _currentWindow)
            {
                if (_currentWindow != null)
                {
                    _currentWindow.Close();
                }

                _currentWindow = window;
                if (foundWindow)
                {
                    //_currentWindow.LoadVideo();
                    _currentWindow.Open();
                }
            }
        }

        Window _currentWindow;
        bool _active;
        int _currentlyLoadingWindow;
        static readonly WaitForSeconds _waitForSecond = new (1);
    }
}
