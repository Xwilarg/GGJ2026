using UnityEngine;

namespace GGJ2026.Player
{
    public class PlayerFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _toFollow;

        private Vector3 _offset;

        private void Awake()
        {
            _offset = _toFollow.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = _toFollow.position - _offset;
        }
    }
}
