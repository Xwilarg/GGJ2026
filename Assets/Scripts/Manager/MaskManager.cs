using GGJ2026.Level;
using GGJ2026.Player;
using Sketch.Translation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GGJ2026.Manager
{
    public class ButtonMask
    {
        public Button Button { set; get; }
        public MaskType MaskType { set; get; }
    }

    public class MaskManager : MonoBehaviour
    {
        public static MaskManager Instance { private set; get; }

        [SerializeField]
        private AudioSource _maskSfx;

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

                    _maskSfx.PlayOneShot(GameManager.Instance.GetMask(value).SFX);
                }
            }
        }
        public UnityEvent<MaskType> OnMaskChange { private set; get; } = new();

        private readonly List<ButtonMask> _availableMasks = new();

        public void AddMask(Button b, MaskType mask)
        {
            _availableMasks.Add(new() { Button = b, MaskType = mask });
        }

        public ButtonMask TryGetMask(int index)
        {
            if (index >= _availableMasks.Count) return null;
            return _availableMasks[index];
        }

        private readonly List<MaskAttachedArea> _attachedAreas = new();

        private void Awake()
        {
            Instance = this;
        }

        public void Register(MaskAttachedArea area)
        {
            _attachedAreas.Add(area);
        }
    }
}
