using CustomMath;
using Unity.VisualScripting;
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
    public bool GetRoom(Room room, Vec3 point)
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].IsInsideRoom(point))
            {
                room = rooms[i];
                return true;
            }
        }

        return false;
    }

    public bool AreAtContiguousRooms(Vec3 point1, Vec3 point2)
    {
        GetRoom(point1);
    }
}
