using GGJ2026.Map;
using GGJ2026.SO;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ2026.Manager
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _roomPrefab;

        [SerializeField]
        private MapInfo _genInfo;

        private readonly List<Room> _rooms = new();

        private const int RoomSize = 20;

        private void Awake()
        {
            for (int depth = 1; depth <= _genInfo.GenerationDepth; depth++)
            {
                for (int x = 0; x < depth; x++)
                {
                    var xPos = x * RoomSize;
                    var yPos = (depth - x - 1) * RoomSize;
                    GenerateRoom(xPos, yPos);
                }
            }
        }

        private Room GenerateRoom(int x, int z)
        {
            var go = Instantiate(_roomPrefab, new Vector3(x, 0f, z), Quaternion.identity);

            return new Room()
            {
                RR = go.GetComponent<RuntimeRoom>()
            };
        }
    }
}
