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

        private const int RoomSize = 30;

        private void Awake()
        {
            Room startingRoom = null;

            Dictionary<int, Room> tmpRooms = new();
            for (int depth = 1; depth <= _genInfo.GenerationDepth; depth++)
            {
                for (int x = 0; x < depth; x++)
                {
                    var xPos = x * RoomSize;
                    var yPos = (depth - x - 1) * RoomSize;
                    var pos = new Vector2Int(xPos, yPos);
                    var room = GenerateRoom(xPos, yPos);

                    if (startingRoom == null)
                    {
                        startingRoom = room;
                    }
                    else
                    {
                        if (tmpRooms.TryGetValue(new Vector2Int(xPos - RoomSize, yPos).GetHashCode(), out var xRoom)) xRoom.LeftDoor = new Door() { LinkedRoom = room };
                        if (tmpRooms.TryGetValue(new Vector2Int(xPos, yPos - RoomSize).GetHashCode(), out var yRoom)) yRoom.RightDoor = new Door() { LinkedRoom = room };
                    }

                    tmpRooms.Add(pos.GetHashCode(), room);
                    _rooms.Add(room);
                }
            }

            foreach (var r in _rooms)
            {
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            foreach (var r in _rooms)
            {
                if (r.LeftDoor != null) Gizmos.DrawLine(r.RR.transform.position, r.LeftDoor.LinkedRoom.RR.transform.position);
                if (r.RightDoor != null) Gizmos.DrawLine(r.RR.transform.position, r.RightDoor.LinkedRoom.RR.transform.position);
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
