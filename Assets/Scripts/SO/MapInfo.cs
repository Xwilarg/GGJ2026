using UnityEngine;

namespace GGJ2026.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/MapInfo", fileName = "MapInfo")]
    public class MapInfo : ScriptableObject
    {
        public int GenerationDepth;
    }
}