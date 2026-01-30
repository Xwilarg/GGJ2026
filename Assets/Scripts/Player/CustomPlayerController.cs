using Sketch.FPS;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GGJ2026.Player
{
    public class CustomPlayerController : PlayerController
    {

        protected override void Awake()
        {
            base.Awake();
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
