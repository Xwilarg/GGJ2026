using GGJ2026.Map;
using GGJ2026.Player;
using GGJ2026.SO;
using Sketch.Translation;
using System.Collections.Generic;
using System.Linq;
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

            void AddRoom(int depth, int x)
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

            for (int depth = 1; depth <= _genInfo.GenerationDepth; depth++)
            {
                for (int x = 0; x < depth; x++)
                {
                    AddRoom(depth, x);
                }
            }
            int tmp = _genInfo.GenerationDepth;
            int off = 0;
            for (int depth = _genInfo.GenerationDepth + 1; depth <= _genInfo.GenerationDepth * 2; depth++/*, tmp--*/, off++)
            {
                for (int x = off; x < tmp; x++)
                {
                    AddRoom(depth, x);
                }
            }

            var maskTypes = System.Enum.GetValues(typeof(MaskType)).Cast<MaskType>().ToArray();

            foreach (var r in _rooms)
            {
                if (r.LeftDoor != null)
                {
                    r.LeftDoor.Requirement = maskTypes[Random.Range(0, maskTypes.Length)];

                    var req = GameManager.Instance.GetMask(r.LeftDoor.Requirement);
                    r.RR.LeftMirror.AssociatedLine = Translate.Instance.Tr("see_intro", Translate.Instance.Tr($"{req.BaseLine}_{Random.Range(1, req.LineCount + 1)}"));
                    r.RR.LeftMirror.SetAssociatedMask(r.LeftDoor.Requirement);
                }
                if (r.RightDoor != null)
                {
                    r.RightDoor.Requirement = maskTypes[Random.Range(0, maskTypes.Length)];

                    var req = GameManager.Instance.GetMask(r.RightDoor.Requirement);
                    r.RR.RightMirror.AssociatedLine = Translate.Instance.Tr("see_intro", Translate.Instance.Tr($"{req.BaseLine}_{Random.Range(1, req.LineCount + 1)}"));
                    r.RR.RightMirror.SetAssociatedMask(r.RightDoor.Requirement);
                }
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
