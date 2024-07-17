using UnityEngine;

namespace Showcase
{
    public class ControlsHint : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] float _fadeDuration = 0.6f;

        void Update()
        {
            if (!_fading)
            {
                _fading |= Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
                return;
            }

            var alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0, Time.deltaTime / _fadeDuration);
            _canvasGroup.alpha = alpha;
            if (alpha <= 0)
            {
                _fading = false;
                Destroy(gameObject);
            }
        }

        bool _fading;
    }
}