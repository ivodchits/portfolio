using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Showcase
{
    public class Window : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] CustomVideoPlayer _videoPlayer;
        [SerializeField] bool _loadVideo = true;

        public Transform Transform { get; private set; }
        public bool ShouldLoadVideo => _loadVideo;

        void Start()
        {
            Transform = transform;
        }

        public void LoadVideo()
        {
            _videoPlayer.LoadFrames();
        }

        public void Open()
        {
            _animator.SetBool(_open, true);
        }

        public void Close()
        {
            _animator.SetBool(_open, false);
        }

        static readonly int _open = Animator.StringToHash("Open");
    }
}
