using GGJ2026.Manager;
using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.Level
{
    public class MaskAttachedArea : MonoBehaviour
    {
        [SerializeField]
        private MaskType _maskType;

        private Renderer _renderer;
        private Collider _coll;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _coll = GetComponent<Collider>();
        }

        private void Start()
        {
            MaskManager.Instance.Register(this);
            MaskManager.Instance.OnMaskChange.AddListener((value) =>
            {
                ToggleObject(value == _maskType);
            });
            ToggleObject(MaskManager.Instance.CurrentMask == _maskType);
        }

        private void ToggleObject(bool value)
        {
            _renderer.enabled = value;
            _coll.enabled = value;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            if (_coll is BoxCollider bColl)
            {
                Gizmos.DrawWireCube(transform.position, bColl.size);
            }
            else Debug.LogWarning($"Unknown collider debug for {name}");
        }
    }
}
