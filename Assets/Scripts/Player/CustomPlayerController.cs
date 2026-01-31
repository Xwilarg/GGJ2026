using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

namespace GGJ2026.Player
{
    public class CustomPlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed, _jumpForce;

        private SpriteRenderer _sr;
        private Rigidbody _rb;
        private Vector2 _rawMov;
        private Camera _cam;

        private void Awake()
        {
            _sr = GetComponentInChildren<SpriteRenderer>();
            _rb = GetComponent<Rigidbody>();
            _cam = Camera.main;
        }

        private void Update()
        {
            if (_rawMov.x != 0f)
            {
                _sr.flipX = _rawMov.x > 0f;
            }
        }

        private void FixedUpdate()
        {
            Vector3 mov = _cam.transform.forward * _rawMov.y + _cam.transform.right * _rawMov.x;
            mov.y = 0f;

            _rb.linearVelocity = mov.normalized * _speed;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _rawMov = value.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started)
            {

            }
        }
    }
}
