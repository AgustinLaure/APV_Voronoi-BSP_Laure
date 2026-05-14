using UnityEngine;
using System.Collections.Generic;
using CustomMath;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;

public class RenderOptimization : MonoBehaviour
{
    public const float epsilon = 1E-05f;

    [SerializeField] private Camera camera;
    [SerializeField] private Building building;
    private List<Window> windows = new List<Window>();
    private List<Vec3> debugPartitions = new List<Vec3>();

    [SerializeField] private float horizontalFov;
    [SerializeField] private float verticalFov;

    private const int precisionX = 5;
    private const int precisionY = 2;

    [SerializeField] private float rayDistance = 1f;
    private Vec3[] raysDir = new Vec3[precisionX * precisionY];
    private Vec3[] raysCollisionPoint = new Vec3[precisionX * precisionY];

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
        building.ResetIsDrawable();
        debugPartitions.Clear();

        Vec3 firstFrom = new Vec3(camera.transform.position);
        int startingRoomIndex = 0;

        if (building.GetRoomIndex(ref startingRoomIndex, firstFrom))
        {
            building.rooms[startingRoomIndex].isDrawable = true;
        }

        for (int i = 0; i < raysCollisionPoint.Length; i++)
        {
            Vec3 firstTo = raysCollisionPoint[i];

            if (building.GetRoomIndex(ref startingRoomIndex, firstTo))
            {
                building.rooms[startingRoomIndex].isDrawable = true;
            }

            if (!building.AreAtContiguousRooms(firstFrom, firstTo))
            {
                Bsp(firstFrom, firstFrom, firstTo, i);
            }
        }
    }

    private void Bsp(Vec3 current, Vec3 from, Vec3 to, int rayIndex)
    {
        debugPartitions.Add(current);

        int currentRoomIndex = 0;
        building.GetRoomIndex(ref currentRoomIndex, current);

        building.rooms[currentRoomIndex].isDrawable = true;

        Vec3 nextPartitionDir = Vec3.Zero;

        if (building.AreAtContiguousRooms(current, from) && building.AreAtContiguousRooms(current, to))
        {
            return;
        }
        else if (building.AreAtContiguousRooms(current, from))
        {
            nextPartitionDir = to - current;
            from = current;
        }
        else if (building.AreAtContiguousRooms(current, to))
        {
            nextPartitionDir = from - current;
            to = current;
        }

        Vec3 point = current + nextPartitionDir * 0.5f;
        current = point;

        Bsp(current, from, to, rayIndex);
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
        float wideness = Mathf.Tan(horizontalFov * Mathf.Deg2Rad * 0.5f);
        float height = Mathf.Tan(verticalFov * Mathf.Deg2Rad * 0.5f);

        Vec3 bottomLeft = new Vec3(camera.transform.position + camera.transform.forward + -camera.transform.right * wideness + -camera.transform.up * height);

        float wideIter = wideness * 2 / precisionX;
        float heightIter = height * 2 / precisionY;

        int rays = 0;
        for (int i = 0; i < precisionX; i++)
        {
            for (int j = 0; j < precisionY; j++)
            {
                Vec3 vec = new Vec3(bottomLeft + camera.transform.right * wideIter * i + camera.transform.up * heightIter * j);
                vec -= new Vec3(camera.transform.position);
                vec.Normalize();

                raysDir[rays] = vec;
                rays++;
            }
        }
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

        foreach (Vec3 partition in debugPartitions)
        {
            Gizmos.DrawSphere(partition, 0.05f);
        }
    }

    public void OnDrawGizmos()
    {
        DrawRays();
    }
}
