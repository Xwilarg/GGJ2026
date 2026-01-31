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
    }
}