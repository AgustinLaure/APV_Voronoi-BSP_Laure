using CustomMath;
using UnityEngine;

public class Room
{
    public MyPlane[] planes = new MyPlane[6];
    public Vec3[] wallsCenter = new Vec3[6];

    public Room(GameObject room)
    {
        BoxCollider roomCollider = room.GetComponent<BoxCollider>();

        Vec3 roomCenter = new Vec3(room.transform.position);
        Vec3 roomExtents = new Vec3(roomCollider.bounds.extents);
        Vec3 currentWallCenter;
        Vec3 currentDirToCenter;

        wallsCenter[0] = roomCenter + (new Vec3(-room.transform.right) * roomExtents.x);
        currentWallCenter = wallsCenter[0];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[0] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[1] = roomCenter + (new Vec3(room.transform.right) * roomExtents.x);
        currentWallCenter = wallsCenter[1];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[1] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[2] = roomCenter + (new Vec3(-room.transform.up) * roomExtents.x);
        currentWallCenter = wallsCenter[2];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[2] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[3] = roomCenter + (new Vec3(room.transform.up) * roomExtents.x);
        currentWallCenter = wallsCenter[3];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[3] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[4] = roomCenter + (new Vec3(-room.transform.forward) * roomExtents.x);
        currentWallCenter = wallsCenter[4];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[4] = new MyPlane(currentDirToCenter, currentWallCenter);

        wallsCenter[5] = roomCenter + (new Vec3(room.transform.forward) * roomExtents.x);
        currentWallCenter = wallsCenter[5];
        currentDirToCenter = roomCenter - currentWallCenter;
        planes[5] = new MyPlane(currentDirToCenter, currentWallCenter);

    }
}
