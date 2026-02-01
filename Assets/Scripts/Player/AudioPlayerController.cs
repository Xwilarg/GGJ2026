using GGJ2026.Manager;
using UnityEngine;

namespace GGJ2026.Player
{
    public class AudioPlayerController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _bgm;
        public ArrangementSwitchingQuantized MusicManager;

        private void Start()
        {
            MaskManager.Instance.OnMaskChange.AddListener((newMaskValue) =>
            {
                // Do mask things here :D
                MusicManager.SetMusicArrangementByMask(newMaskValue);
            });
        }

        public float PlayerYPosition => transform.position.y;
    }
}
