using Sketch.FPS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2026.Player
{
    public class CustomPlayerController : PlayerController
    {
        private SpriteRenderer _sr;
        private Vector2 _rawMov;

        protected override void Awake()
        {
            base.Awake();

            _sr = GetComponentInChildren<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();

            if (_rawMov.x != 0f)
            {
                _sr.flipX = _rawMov.x > 0f;
            }
        }

        public void OnMovementOverrides(InputAction.CallbackContext value)
        {
            _rawMov = value.ReadValue<Vector2>();
            var delta = 45f;

            _mov = new Vector2(
                _rawMov.x * Mathf.Sin(Mathf.Deg2Rad * delta) + _rawMov.y * Mathf.Cos(Mathf.Deg2Rad * delta),
                -(_rawMov.x * Mathf.Cos(Mathf.Deg2Rad * delta) - _rawMov.y * Mathf.Sin(Mathf.Deg2Rad * delta))
            );
        }
    }
}
