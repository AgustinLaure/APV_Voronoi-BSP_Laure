using UnityEngine;
using System.Collections.Generic;
using CustomMath;
using UnityEditor.ShaderGraph;

public class Voronoi : MonoBehaviour
{
    [SerializeField] private GameObject[] regionsObj;
    [SerializeField] private GameObject player;

    private List<Region> regions = new List<Region>();
    private bool isAllSet = false;
    private Color isPlayerInColor;

    public class Region
    {
        public Region(Color color, Vec3 pos)
        {
            this.pos = pos;
            this.color = color;
            this.baseColor = color;
        }

        private bool isPlayerIn = false;
        public bool IsPlayerIn { get { return isPlayerIn; } set { isPlayerIn = value; } }

        private Color baseColor;
        public Color BaseColor { get { return baseColor; } private set { } }

        private Color color;
        public Color Color { get { return color; } set { color = value; } }

        public Vec3 pos;
        public List<MyPlane> planes = new List<MyPlane>();
    }

    private void Awake()
    {
        isPlayerInColor.r = 10f;
        isPlayerInColor.g = 10f;
        isPlayerInColor.b = 10f;
        isPlayerInColor.a = 1f;

        SetRegionsPos();
        SetRegionsPlanes();
        isAllSet = true;
    }

    private void Update()
    {
        UpdateClosestRegionToPlayer();

        for (int i = 0; i < regionsObj.Length; i++)
        {
            Color currentColor = regions[i].BaseColor;

            if (regions[i].IsPlayerIn)
            {
                currentColor = isPlayerInColor;
            }

            regions[i].Color = currentColor;

            regionsObj[i].GetComponent<MeshRenderer>().material.color = regions[i].Color;
        }
    }

    private void UpdateClosestRegionToPlayer()
    {
        Region closestRegionToPlayer = GetClosestRegion(new Vec3(player.transform.position));

        foreach (var region in regions)
        {
            region.IsPlayerIn = region == closestRegionToPlayer;
        }
    }
    private void SetRegionsPos()
    {
        for (int i = 0; i < regionsObj.Length; i++)
        {
            regions.Add(new Region(GetRandomColor(), new Vec3(regionsObj[i].transform.position)));
        }
    }

    private Color GetRandomColor()
    {
        Color randomColor;
        randomColor.r = Random.Range(20, 255) / 255f;
        randomColor.g = Random.Range(20, 255) / 255f;
        randomColor.b = Random.Range(20, 255) / 255f;
        randomColor.a = 1f;

        return randomColor;
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

        RemoveRedundantPlanes();
    }

    private void RemoveRedundantPlanes()
    {
        for (int i = 0; i < regions.Count; i++)
        {
            for (int j = 0; j < regions[i].planes.Count; j++)
            {
                if (IsPlaneRedundant(regions[i], regions[i].planes[j]))
                {
                    regions[i].planes.Remove(regions[i].planes[j]);
                }
            }
        }
    }

    private bool IsPlaneRedundant(Region region, MyPlane toCheckPlane)
    {
        Vec3 toCheckPlaneClosestPoint = toCheckPlane.ClosestPointOnPlane(region.pos);

        foreach (MyPlane regionPlane in region.planes)
        {
            if (!regionPlane.GetSide(toCheckPlaneClosestPoint))
            {
                return true;
            }
        }

        return false;
    }

    private MyPlane GetPlaneInBetween(Vec3 from, Vec3 to)
    {
        Vec3 middlePoint = from + (to - from) / 2;
        Vec3 planeNormal = from - middlePoint;

        return new MyPlane(planeNormal, middlePoint);
    }

    private void DrawPlaneFromRegion(MyPlane plane, Vec3 regionPos, Color color)
    {
        Vec3 planeCenter = plane.ClosestPointOnPlane(regionPos);
        Gizmos.color = color;

        Gizmos.DrawLine(planeCenter, planeCenter + plane.Normal);

        //draw tip
        Gizmos.DrawSphere(planeCenter + plane.Normal, 0.05f);

        Gizmos.color = default;
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
                DrawPlaneFromRegion(regions[i].planes[j], regions[i].pos, regions[i].Color);
            }
        }
    }
}
