using UnityEngine;

namespace Showcase
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] Transform _transform;
        [SerializeField] Rigidbody2D _rigidbody;
        [SerializeField] float _speed = 5f;
        [SerializeField] float _minX = -5f;
        [SerializeField] float _maxX = 60f;

        void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            if (_transform.position.x < _minX && horizontal < 0 || _transform.position.x > _maxX && horizontal > 0)
                horizontal = 0;

            _rigidbody.velocityX = horizontal * _speed;

            if (Mathf.Abs(horizontal) > 0.1f)
            {
                _transform.localScale = Mathf.Sign(horizontal) > 0 ? _rightScale : _leftScale;
            }
        }

        static readonly Vector3 _leftScale = new(-1, 1, 1);
        static readonly Vector3 _rightScale = new(1, 1, 1);
    }
}
