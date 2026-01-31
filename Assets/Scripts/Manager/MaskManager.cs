using GGJ2026.Level;
using GGJ2026.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GGJ2026.Manager
{
    public class RuntimeMaskData
    {
        public MaskType MaskType { set; get; }
        public Button Button { set; get; }
    }

    public class MaskManager : MonoBehaviour
    {
        public static MaskManager Instance { private set; get; }

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

        private readonly List<RuntimeMaskData> _availableMasks = new();

        public void AddMask(MaskType m, Button b)
        {
            _availableMasks.Add(new() { MaskType = m, Button = b });
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
