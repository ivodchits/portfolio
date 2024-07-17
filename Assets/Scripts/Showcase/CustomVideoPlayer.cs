using UnityEngine;
using UnityEngine.UI;

namespace Showcase
{
    public class CustomVideoPlayer : MonoBehaviour
    {
        [SerializeField] Image _image;
        [SerializeField] SpriteRenderer _loadingIcon;
        [SerializeField] SpriteRenderer _logoIcon;
        [SerializeField] FrameCollection _frameCollection;

        public bool Loaded => _frameCollection.Loaded;

        void OnEnable()
        {
            _enabled = true;
            _startTime = Time.time;
        }

        void OnDisable()
        {
            _loadingIcon.enabled = false;
            _logoIcon.enabled = true;
            _enabled = false;
        }

        public void LoadFrames()
        {
            _frameCollection.Load();
        }

        void Update()
        {
            if (!_enabled)
                return;

            if (!Loaded)
            {
                _logoIcon.enabled = true;
                _loadingIcon.enabled = true;
                _image.enabled = false;
                _loadingIcon.transform.rotation = Quaternion.AngleAxis(Mathf.Floor(Mathf.Repeat(Time.time, 1) * 8) * 45, Vector3.forward);
                return;
            }

            _logoIcon.enabled = false;
            _loadingIcon.enabled = false;
            _image.enabled = true;

            var frameIndex = (int)((Time.time - _startTime) * 60) % _frameCollection.Length;
            _image.sprite = _frameCollection.Get(frameIndex);
        }

        bool _enabled;
        float _startTime;
    }
}