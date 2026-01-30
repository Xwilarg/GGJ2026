using Sketch.FPS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GGJ2026.Player
{
    public class CustomPlayerController : PlayerController
    {
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
        }

        public void OnMovementOverrides(InputAction.CallbackContext value)
        {
            float rot = 45f;

            var mov = value.ReadValue<Vector2>();
            _mov = new Vector2(
                mov.x * Mathf.Cos(rot) - mov.y * Mathf.Sin(rot),
                mov.x * Mathf.Sin(rot) + mov.y * Mathf.Cos(rot)
            );
        }
    }
}
