using UnityEngine;

namespace Showcase
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] float _smoothTime = 0.3f;
        [SerializeField] float _minX = -6;
        [SerializeField] float _maxX = 60;

        void Start()
        {
            _transform = transform;
        }

        void Update()
        {
            var currentVelocity = Vector3.zero;
            var oldPosition = _transform.position;
            var position = Vector3.SmoothDamp(oldPosition, _target.position, ref currentVelocity, _smoothTime);
            position.x = Mathf.Clamp(position.x, _minX, _maxX);
            position.y = oldPosition.y;
            position.z = oldPosition.z;
            _transform.position = position;
        }

        Transform _transform;
    }
}
