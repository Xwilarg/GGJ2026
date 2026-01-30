using UnityEngine;

namespace GGJ2026.Player
{
    public class AudioPlayerController : MonoBehaviour
    {
        #region Init
        private CustomPlayerController _player;

        private void Awake()
        {
            _player = GetComponent<CustomPlayerController>();
        }
        #endregion

        public float PlayerYPosition => transform.position.y;
    }
}
