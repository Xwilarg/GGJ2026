using GGJ2026.Player;
using UnityEngine;

namespace GGJ2026.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        public float MovementSpeed;
        public float JumpForce;
        public float MinDistanceWithFloorForJump;
        public float SimulatedGravityForce;
        public float FootstepInterval;

        // public MaskType StartingMask;
    }

    [System.Serializable]
    public class MaskInfo
    {
        public MaskType Type;
        public Sprite Sprite;

        public string BaseLine;
        public int LineCount;

        public AudioClip SFX;
    }
}