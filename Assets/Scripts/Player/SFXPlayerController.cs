using UnityEngine;

namespace GGJ2026.Player
{
    public class SFXPlayerController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _footstepsSfx, _jumpSfx, _jumpLandSfx;
        [SerializeField]
        private AudioClip[] _footstepsClips, _jumpClips, _jumpLandClips;

        public void PlayRandomFootstep()
        {
            _footstepsSfx.PlayOneShot(_footstepsClips[Random.Range(0, _footstepsClips.Length)]);
        }
        public void PlayRandomJump()
        {
            _jumpSfx.PlayOneShot(_jumpClips[Random.Range(0, _jumpClips.Length)]);
        }
        public void PlayRandomJumpLand()
        {
            _jumpLandSfx.PlayOneShot(_jumpLandClips[Random.Range(0, _jumpLandClips.Length)]);
        }
    }
}
