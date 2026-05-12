using UnityEngine;
using CustomMath;
public class Player : MonoBehaviour
{
    [SerializeField] Voronoi voronoi;
    private void Update()
    {
        Vec3 closestRegionPos = voronoi.GetClosestRegion(new Vec3(transform.position)).pos;
        Debug.Log("Im in region: " + closestRegionPos.ToString());
    }
}
