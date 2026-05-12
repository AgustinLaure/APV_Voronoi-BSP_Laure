using UnityEngine;
using System.Collections.Generic;
using CustomMath;
using System.IO.IsolatedStorage;
using System.Drawing;
public class Voronoi : MonoBehaviour
{
    [SerializeField] private Transform[] regionsTrs;

    private List<Region> regions = new List<Region>();
    private bool isAllSet = false;

    public class Region
    {
        public Vec3 pos;
        public List<MyPlane> planes = new List<MyPlane>();
    }

    private void Awake()
    {
        SetRegionsPos();
        SetRegionsPlanes();
        isAllSet = true;
    }

    private void SetRegionsPos()
    {
        for (int i = 0; i < regionsTrs.Length; i++)
        {
            regions.Add(new Region());
            regions[i].pos = new Vec3(regionsTrs[i].transform.position);
        }
    }

    private void SetRegionsPlanes()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            for (int j = 0; j < regions.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                regions[i].planes.Add(GetPlaneInBetween(regions[i].pos, regions[j].pos));
            }
        }
    }

    private MyPlane GetPlaneInBetween(Vec3 from, Vec3 to)
    {
        Vec3 middlePoint = from + (to - from) / 2;
        Vec3 planeNormal = from - middlePoint;

        return new MyPlane(planeNormal, middlePoint);
    }

    private void DrawPlane(MyPlane plane)
    {
        Vec3 planeCenter = plane.Normal * plane.Distance;
        Gizmos.DrawLine(planeCenter, planeCenter + plane.Normal);

        //draw tip
        Gizmos.DrawSphere(planeCenter + plane.Normal, 0.05f);
    }

    public Region GetClosestRegion(Vec3 point)
    {
        for (int i = 0; i < regions.Count; i++)
        {
            if (IsInRegion(regions[i], point))
            {
                return regions[i];
            }
        }
        return regions[0];
    }

    private bool IsInRegion(Region region, Vec3 point)
    {
        bool isInRegion = true;

        for (int i = 0; i < region.planes.Count; i++)
        {
            if (!region.planes[i].GetSide(point))
            {
                isInRegion = false;
            }
        }

        return isInRegion;
    }
    private void OnDrawGizmos()
    {
        if (!isAllSet)
        {
            return;
        }

        for (int i = 0; i < regions.Count; i++)
        {
            for (int j = 0; j < regions[i].planes.Count; j++)
            {
                DrawPlane(regions[i].planes[j]);
            }
        }
    }
}
