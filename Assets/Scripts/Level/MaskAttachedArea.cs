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

        private Color[] _colors = new[] { Color.red, Color.green, Color.blue };
        private void OnDrawGizmos()
        {
            Gizmos.color = _colors[(int)_maskType];
            if (GetComponent<Collider>() is BoxCollider bColl)
            {
                Gizmos.DrawWireCube(transform.position + bColl.center, new Vector3(bColl.size.x * transform.localScale.x, bColl.size.y * transform.localScale.y, bColl.size.z * transform.localScale.z));
            }
            else Debug.LogWarning($"Unknown collider debug for {name}");
        }
    }
}
