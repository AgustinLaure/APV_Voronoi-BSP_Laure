using CustomMath;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] GameObject[] roomsObj;

    List<Room> rooms = new List<Room>();

    private void Start()
    {
        SetRooms();
    }

    private void SetRooms()
    {
        for (int i = 0; i < roomsObj.Length; i++)
        {
            rooms.Add(new Room(roomsObj[i]));
        }
    }

    public void DrawPlaneFromPoint(MyPlane plane, Vec3 point, Color color)
    {
        Vec3 planeCenter = point;
        Gizmos.color = color;

        Vec3 planeNormal = plane.Normal;
        Gizmos.DrawLine(planeCenter, planeCenter + plane.Normal);

        //draw tip
        Gizmos.DrawSphere(planeCenter + plane.Normal, 0.05f);

        Gizmos.color = default;
    }

    public void DrawRoomWireframe(GameObject room)
    {
        MeshFilter roomMesh = room.GetComponent<MeshFilter>();
        Gizmos.DrawWireMesh(roomMesh.sharedMesh, room.transform.position, room.transform.rotation, room.transform.localScale);
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < rooms[i].planes.Length; j++)
            {
                DrawPlaneFromPoint(rooms[i].planes[j], rooms[i].wallsCenter[j], Color.red);
            }

            //DrawRoomWireframe(roomsObj[i]);
        }
    }
}
