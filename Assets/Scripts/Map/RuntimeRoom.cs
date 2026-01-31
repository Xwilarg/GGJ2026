using GGJ2026.Prop;
using UnityEngine;

namespace GGJ2026.Map
{
    public class RuntimeRoom : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _presets;

        [SerializeField]
        private Mirror _leftMirror, _rightMirror;
        public Mirror LeftMirror => _leftMirror;
        public Mirror RightMirror => _rightMirror;

        private void Awake()
        {
            foreach (var p in _presets)
            {
                p.SetActive(false);
            }
            _presets[Random.Range(0, _presets.Length)].SetActive(true);
        }
    }
}
