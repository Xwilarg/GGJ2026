using Sketch.FPS;
using UnityEngine.Events;

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

        private void Awake()
        {
        }
    }
}
