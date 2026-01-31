using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2026.Player
{
    public class CustomPlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed, _jumpForce, _jumpDist;

        private SpriteRenderer _sr;
        private Rigidbody _rb;
        private Vector2 _rawMov;
        private Camera _cam;

        private bool _canJump = true;

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

        private bool CanJump => _canJump && Physics.Raycast(transform.position, Vector3.down, _jumpForce, LayerMask.GetMask("World"));

        public void OnMove(InputAction.CallbackContext value)
        {
            _rawMov = value.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && CanJump)
            {
                _canJump = false;
                _rb.linearVelocity = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                StartCoroutine(RefreshJump());
            }
        }

        private IEnumerator RefreshJump()
        {
            yield return new WaitForSeconds(1f);
            _canJump = true;
        }
    }
}
