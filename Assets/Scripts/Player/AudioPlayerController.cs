using UnityEngine;

namespace GGJ2026.Player
{
    public class AudioPlayerController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _bgm;

        private CustomPlayerController _player;

        private void Awake()
        {
            _player = GetComponent<CustomPlayerController>();

            _player.OnMaskChange.AddListener((newMaskValue) =>
            {
                // Do mask things here :D
            });
        }

        public float PlayerYPosition => transform.position.y;
    }
}
