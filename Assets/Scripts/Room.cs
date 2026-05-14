using CustomMath;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //[SerializeField] GameObject[] items;

    public bool isDrawable = true;
    public MyPlane[] planes = new MyPlane[6];
    public Vec3[] wallsCenter = new Vec3[6];
    [SerializeField] private GameObject[] items;
    [SerializeField] public List<Room> neighbors = new List<Room>();
    [SerializeField] public List<Window> windows = new List<Window>();
    private void Update()
    {
        Debug.Log(isDrawable);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<Renderer>().enabled = isDrawable;
        }
    }
    private void Awake()
    {
        BoxCollider roomCollider = GetComponent<BoxCollider>();

        Vec3 roomCenter = new Vec3(transform.position);
        Vec3 roomExtents = new Vec3(roomCollider.bounds.extents);
        Vec3 currentWallCenter;
        Vec3 currentDirToCenter;

        wallsCenter[0] = roomCenter + (new Vec3(-transform.right) * roomExtents.x);
        currentWallCenter = wallsCenter[0];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[0] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[1] = roomCenter + (new Vec3(transform.right) * roomExtents.x);
        currentWallCenter = wallsCenter[1];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[1] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[2] = roomCenter + (new Vec3(-transform.up) * roomExtents.y);
        currentWallCenter = wallsCenter[2];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[2] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[3] = roomCenter + (new Vec3(transform.up) * roomExtents.y);
        currentWallCenter = wallsCenter[3];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[3] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[4] = roomCenter + (new Vec3(-transform.forward) * roomExtents.z);
        currentWallCenter = wallsCenter[4];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[4] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[5] = roomCenter + (new Vec3(transform.forward) * roomExtents.z);
        currentWallCenter = wallsCenter[5];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[5] = new MyPlane(currentDirToCenter, currentWallCenter);
    }

    public bool IsInsideRoom(Vec3 point)
    {
        bool isInside = true;

        for (int i = 0; i < planes.Length; i++)
        {
            if (!planes[i].GetSide(point))
            {
                isInside = false;
                break;
            }
        }

        return isInside;
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

    public void OnDrawGizmos()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            DrawPlaneFromPoint(planes[i], wallsCenter[i], Color.red);
        }
        //DrawRoomWireframe(roomsObj[i]);
    }
}
