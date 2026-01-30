using Sketch.FPS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

namespace GGJ2026.Player
{
    public class CustomPlayerController : PlayerController
    {
        private Camera _cam;

        private MaskType _currentMask;
        public MaskType CurrentMask
        {
            get => _currentMask;
            set
            {
                if (value != _currentMask)
                {
                    _currentMask = value;
                    OnMaskChange.Invoke(value);
                }
            }
        }
        public UnityEvent<MaskType> OnMaskChange { private set; get; } = new();

        protected override void Awake()
        {
            base.Awake();

            _cam = Camera.main;
        }

        public void OnMovementOverrides(InputAction.CallbackContext value)
        {
            var mov = value.ReadValue<Vector2>();
            var delta = 45f;

            _mov = new Vector2(
                mov.x * Mathf.Sin(Mathf.Deg2Rad * delta) + mov.y * Mathf.Cos(Mathf.Deg2Rad * delta),
                -(mov.x * Mathf.Cos(Mathf.Deg2Rad * delta) - mov.y * Mathf.Sin(Mathf.Deg2Rad * delta))
            );
        }
    }
}
