using GGJ2026.SO;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2026.Player
{
    public class CustomPlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private SpriteRenderer _sr;
        private Rigidbody _rb;
        private Vector2 _rawMov;
        private Camera _cam;
        private float _yJumpForce;

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

            if (_yJumpForce > 0f)
            {
                _yJumpForce -= Time.deltaTime * _info.SimulatedGravityForce;
                _rb.AddForce(Vector3.up * _yJumpForce);
            }
        }

        private void FixedUpdate()
        {
            Vector3 mov = _cam.transform.forward * _rawMov.y + _cam.transform.right * _rawMov.x;
            mov.y = 0f;

            _rb.linearVelocity = mov.normalized * _info.MovementSpeed;
        }

        private bool CanJump => _canJump && Physics.Raycast(transform.position, Vector3.down, _info.MinDistanceWithFloorForJump, LayerMask.GetMask("World"));

        private void OnMaskSelect(InputAction.CallbackContext value, int key)
        {

        }

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
                _yJumpForce = _info.JumpForce;
                StartCoroutine(RefreshJump());
            }
        }

        public void OnMaskSelect1(InputAction.CallbackContext value) => OnMaskSelect(value, 1);
        public void OnMaskSelect2(InputAction.CallbackContext value) => OnMaskSelect(value, 2);
        public void OnMaskSelect3(InputAction.CallbackContext value) => OnMaskSelect(value, 3);
        public void OnMaskSelect4(InputAction.CallbackContext value) => OnMaskSelect(value, 4);
        public void OnMaskSelect5(InputAction.CallbackContext value) => OnMaskSelect(value, 5);
        public void OnMaskSelect6(InputAction.CallbackContext value) => OnMaskSelect(value, 6);
        public void OnMaskSelect7(InputAction.CallbackContext value) => OnMaskSelect(value, 7);
        public void OnMaskSelect8(InputAction.CallbackContext value) => OnMaskSelect(value, 8);
        public void OnMaskSelect9(InputAction.CallbackContext value) => OnMaskSelect(value, 9);

        private IEnumerator RefreshJump()
        {
            yield return new WaitForSeconds(1f);
            _canJump = true;
        }
    }
}
