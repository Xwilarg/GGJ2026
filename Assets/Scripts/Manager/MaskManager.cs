using GGJ2026.Level;
using GGJ2026.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ2026.Manager
{
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
