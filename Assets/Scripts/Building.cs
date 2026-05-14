using CustomMath;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] public Room[] rooms;

    public void ResetIsDrawable()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i].isDrawable = false;
        }
    }
    public bool GetRoomIndex(ref int index, Vec3 point)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsInsideRoom(point))
            {
                index = i;
                return true;
            }
        }

        return false;
    }

    public bool AreAtContiguousRooms(Vec3 point1, Vec3 point2)
    {
        bool areAtContiguousRooms = false;

        int room1Index = 0;
        int room2Index = 0;

        if (!GetRoomIndex(ref room1Index, point1) || !GetRoomIndex(ref room2Index, point2))
        {
            return true;
        }

        if (room1Index == room2Index)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < rooms[room1Index].neighbors.Count; i++)
            {
                if (rooms[room1Index].neighbors[i] == rooms[room2Index])
                {
                    return true;
                }
            }

            for (int i = 0; i < rooms[room2Index].neighbors.Count; i++)
            {
                if (rooms[room2Index].neighbors[i] == rooms[room1Index])
                {
                    return true;
                }
            }
        }

        return areAtContiguousRooms;
    }
}
