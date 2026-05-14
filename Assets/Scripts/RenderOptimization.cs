using UnityEngine;
using System.Collections.Generic;
using CustomMath;

public class RenderOptimization : MonoBehaviour
{
    public const float epsilon = 1E-05f;

    [SerializeField] private Camera camera;
    [SerializeField] private Building building;
    private List<Window> windows = new List<Window>();

    private const int precisionX = 5;
    private const int precisionY = 2;

    [SerializeField] private float rayDistance = 1f;
    private Vec3[] raysDir = new Vec3[1];
    private Vec3[] raysCollisionPoint = new Vec3[1];

    void Start()
    {
        SetWindowsRef();
        CalculateRays();
    }

    private void SetWindowsRef()
    {
        for (int i = 0; i < building.rooms.Length; i++)
        {
            for (int j = 0; j < building.rooms[i].windows.Count; j++)
            {
                windows.Add(building.rooms[i].windows[j]);
            }
        }
    }

    private void SetDrawableRooms()
    {
        for (int i = 0; i < raysCollisionPoint.Length; i++)
        {
            Room currentRoom = new Room();
            Bsp(currentRoom, new Vec3(camera.transform.position), raysCollisionPoint[i]);
        }
    }

    private void Bsp(Room currentRoom, Vec3 from, Vec3 to)
    {
        building.GetRoom(currentRoom, from);

        if (!currentRoom.isDrawable)
        {
            currentRoom.isDrawable = true;
        }

        Vec3 nextPartitionDir = from - to;
        Vec3 point = to + nextPartitionDir * 0.5f;
    }
    private bool SetRayCollisionPoint(MyPlane plane, Vec3 dir, int rayIndex, ref float smallestTValue)
    {
        float alignment = Vec3.Dot(plane.Normal, dir);

        if (alignment < epsilon && alignment > -epsilon)
        {
            return false;
        }

        Vec3 cameraPos = new Vec3(camera.transform.position);

        float ortDist = Vec3.Dot(plane.ClosestPointOnPlane(cameraPos) - cameraPos, plane.Normal);

        float t = ortDist / alignment;

        if (t <= epsilon)
        {
            return false;
        }

        Vec3 newCollisionPoint = new Vec3(camera.transform.position) + (dir * t);

        if (IsCollisionPointAtWindow(newCollisionPoint))
        {
            return false;
        }

        if (t < smallestTValue)
        {
            smallestTValue = t;
            raysCollisionPoint[rayIndex] = newCollisionPoint;
        }

        return true;
    }

    private bool IsCollisionPointAtWindow(Vec3 collisionPoint)
    {
        foreach (Window window in windows)
        {
            if (window.IsInsideWindow(collisionPoint))
            {
                return true;
            }
        }

        return false;
    }

    public void CalculateRays()
    {
        CalculateRaysDir();
        CalculateRaysEnd();
        SetDrawableRooms();
    }
    private void CalculateRaysDir()
    {
        raysDir[0] = new Vec3(camera.transform.forward);
    }
    private void CalculateRaysEnd()
    {
        for (int i = 0; i < raysDir.Length; i++)
        {
            float smallestTValue = float.MaxValue;

            for (int j = 0; j < building.rooms.Length; j++)
            {
                for (int k = 0; k < building.rooms[j].planes.Length; k++)
                {
                    SetRayCollisionPoint(building.rooms[j].planes[k], raysDir[i], i, ref smallestTValue);
                }
            }
        }
    }

    private void DrawRays()
    {
        for (int i = 0; i < raysDir.Length; i++)
        {
            Gizmos.DrawLine(camera.transform.position, raysCollisionPoint[i]);
        }

        for (int i = 0; i < raysCollisionPoint.Length; i++)
        {
            Gizmos.DrawSphere(raysCollisionPoint[i], 0.05f);
        }
    }

    public void OnDrawGizmos()
    {
        DrawRays();
    }
}
